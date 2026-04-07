using PlazaHotels.Models.Entities;
using PlazaHotels.Models.Interfaces;
using PlazaHotels.Models.Factories;

namespace PlazaHotels.Models.Services;

/// <summary>
/// Singleton - Controla disponibilidade, check-in e check-out
/// Implementação thread-safe usando Lazy&lt;T&gt;
/// </summary>
public sealed class GerenciadorReservas
{
    // Lazy<T> garante thread-safety e inicialização sob demanda
    private static readonly Lazy<GerenciadorReservas> _instancia = 
        new(() => new GerenciadorReservas());

    public static GerenciadorReservas Instancia => _instancia.Value;

    private readonly Dictionary<string, Reserva> _reservasAtivas;
    private readonly Dictionary<string, Reserva> _historicoReservas;
    private readonly Dictionary<string, int> _quartosPorTipo;
    private readonly object _lock = new();

    // Construtor privado para impedir instanciação externa
    private GerenciadorReservas()
    {
        _reservasAtivas = new Dictionary<string, Reserva>();
        _historicoReservas = new Dictionary<string, Reserva>();
        _quartosPorTipo = new Dictionary<string, int>
        {
            { "Standard", 20 },
            { "Luxo", 10 },
            { "Suite", 5 }
        };
    }

    #region Verificação de Disponibilidade

    public bool QuartoDisponivel(string tipoQuarto, DateTime dataEntrada, DateTime dataSaida)
    {
        lock (_lock)
        {
            var tipo = NormalizarTipoQuarto(tipoQuarto);
            var reservasConflitantes = _reservasAtivas.Values
                .Where(r => r.TipoQuarto.Equals(tipo, StringComparison.OrdinalIgnoreCase))
                .Where(r => r.Status != StatusReserva.Cancelada)
                .Count(r => DatasConflitam(r.DataEntrada, r.DataSaida, dataEntrada, dataSaida));

            var totalQuartos = _quartosPorTipo.GetValueOrDefault(tipo, 0);
            return reservasConflitantes < totalQuartos;
        }
    }

    public int QuartosDisponiveis(string tipoQuarto, DateTime dataEntrada, DateTime dataSaida)
    {
        lock (_lock)
        {
            var tipo = NormalizarTipoQuarto(tipoQuarto);
            var reservasConflitantes = _reservasAtivas.Values
                .Where(r => r.TipoQuarto.Equals(tipo, StringComparison.OrdinalIgnoreCase))
                .Where(r => r.Status != StatusReserva.Cancelada)
                .Count(r => DatasConflitam(r.DataEntrada, r.DataSaida, dataEntrada, dataSaida));

            var totalQuartos = _quartosPorTipo.GetValueOrDefault(tipo, 0);
            return Math.Max(0, totalQuartos - reservasConflitantes);
        }
    }

    public List<DisponibilidadeQuarto> ObterDisponibilidade(DateTime dataEntrada, DateTime dataSaida)
    {
        var resultado = new List<DisponibilidadeQuarto>();

        foreach (var tipo in _quartosPorTipo.Keys)
        {
            var quarto = QuartoFactory.CriarQuarto(tipo);
            resultado.Add(new DisponibilidadeQuarto
            {
                TipoQuarto = tipo,
                Data = dataEntrada,
                TotalQuartos = _quartosPorTipo[tipo],
                QuartosOcupados = _quartosPorTipo[tipo] - QuartosDisponiveis(tipo, dataEntrada, dataSaida),
                PrecoDiaria = quarto.PrecoDiaria
            });
        }

        return resultado;
    }

    #endregion

    #region Gestão de Reservas

    public Reserva RegistrarReserva(string nomeHospede, string email, string telefone, 
        string tipoQuarto, string tipoPacote, DateTime dataEntrada, DateTime dataSaida, 
        int numeroHospedes, string? observacoes = null)
    {
        lock (_lock)
        {
            var tipo = NormalizarTipoQuarto(tipoQuarto);

            if (!QuartoDisponivel(tipo, dataEntrada, dataSaida))
            {
                throw new InvalidOperationException($"Não há quartos {tipo} disponíveis para o período solicitado.");
            }

            var reserva = new Reserva
            {
                NomeHospede = nomeHospede,
                EmailHospede = email,
                TelefoneHospede = telefone,
                TipoQuarto = tipo,
                TipoPacote = tipoPacote,
                DataEntrada = dataEntrada,
                DataSaida = dataSaida,
                NumeroHospedes = numeroHospedes,
                Observacoes = observacoes,
                Status = StatusReserva.Pendente,
                DataCriacao = DateTime.Now
            };

            // Calcular valor total usando as factories
            var valorTotal = CalcularValorTotal(tipo, tipoPacote, dataEntrada, dataSaida);
            reserva.ValorTotal = valorTotal;

            // Verificar se pacote inclui café e serviço
            if (!string.IsNullOrEmpty(tipoPacote))
            {
                try
                {
                    var factory = PacoteFactoryProvider.ObterFactory(tipoPacote);
                    reserva.CafeDaManhaIncluso = true;
                    reserva.ServicoAdicionalIncluso = true;
                }
                catch
                {
                    reserva.CafeDaManhaIncluso = false;
                    reserva.ServicoAdicionalIncluso = false;
                }
            }

            _reservasAtivas[reserva.Id] = reserva;
            return reserva;
        }
    }

    public bool ConfirmarReserva(string reservaId)
    {
        lock (_lock)
        {
            if (_reservasAtivas.TryGetValue(reservaId, out var reserva))
            {
                reserva.Status = StatusReserva.Confirmada;
                return true;
            }
            return false;
        }
    }

    public bool CancelarReserva(string reservaId)
    {
        lock (_lock)
        {
            if (_reservasAtivas.TryGetValue(reservaId, out var reserva))
            {
                reserva.Status = StatusReserva.Cancelada;
                _historicoReservas[reserva.Id] = reserva;
                _reservasAtivas.Remove(reservaId);
                return true;
            }
            return false;
        }
    }

    #endregion

    #region Check-in e Check-out

    public bool RealizarCheckIn(string reservaId)
    {
        lock (_lock)
        {
            if (_reservasAtivas.TryGetValue(reservaId, out var reserva))
            {
                if (reserva.Status != StatusReserva.Confirmada)
                {
                    throw new InvalidOperationException("A reserva deve estar confirmada para realizar check-in.");
                }

                reserva.Status = StatusReserva.CheckIn;
                return true;
            }
            return false;
        }
    }

    public bool RealizarCheckOut(string reservaId)
    {
        lock (_lock)
        {
            if (_reservasAtivas.TryGetValue(reservaId, out var reserva))
            {
                if (reserva.Status != StatusReserva.CheckIn)
                {
                    throw new InvalidOperationException("A reserva deve estar em check-in para realizar check-out.");
                }

                reserva.Status = StatusReserva.CheckOut;
                _historicoReservas[reserva.Id] = reserva;
                _reservasAtivas.Remove(reservaId);
                return true;
            }
            return false;
        }
    }

    #endregion

    #region Consultas

    public Reserva? ObterReserva(string reservaId)
    {
        lock (_lock)
        {
            return _reservasAtivas.GetValueOrDefault(reservaId) ?? 
                   _historicoReservas.GetValueOrDefault(reservaId);
        }
    }

    public List<Reserva> ListarReservasAtivas()
    {
        lock (_lock)
        {
            return _reservasAtivas.Values
                .Where(r => r.Status != StatusReserva.Cancelada)
                .OrderBy(r => r.DataEntrada)
                .ToList();
        }
    }

    public List<Reserva> ListarReservasPorData(DateTime data)
    {
        lock (_lock)
        {
            return _reservasAtivas.Values
                .Where(r => r.DataEntrada.Date <= data.Date && r.DataSaida.Date >= data.Date)
                .Where(r => r.Status != StatusReserva.Cancelada)
                .ToList();
        }
    }

    public List<Reserva> ListarHistorico()
    {
        lock (_lock)
        {
            return _historicoReservas.Values
                .OrderByDescending(r => r.DataSaida)
                .ToList();
        }
    }

    public Dictionary<string, int> ObterEstatisticas()
    {
        lock (_lock)
        {
            return new Dictionary<string, int>
            {
                { "ReservasAtivas", _reservasAtivas.Count },
                { "ReservasPendentes", _reservasAtivas.Values.Count(r => r.Status == StatusReserva.Pendente) },
                { "ReservasConfirmadas", _reservasAtivas.Values.Count(r => r.Status == StatusReserva.Confirmada) },
                { "HospedesAtuais", _reservasAtivas.Values.Count(r => r.Status == StatusReserva.CheckIn) },
                { "TotalHistorico", _historicoReservas.Count }
            };
        }
    }

    #endregion

    #region Métodos Auxiliares

    private bool DatasConflitam(DateTime inicio1, DateTime fim1, DateTime inicio2, DateTime fim2)
    {
        return inicio1 < fim2 && inicio2 < fim1;
    }

    private string NormalizarTipoQuarto(string tipo)
    {
        return tipo?.ToLower() switch
        {
            "standard" => "Standard",
            "luxo" => "Luxo",
            "suite" or "suíte" or "suíte presidencial" => "Suite",
            _ => tipo ?? "Standard"
        };
    }

    private decimal CalcularValorTotal(string tipoQuarto, string tipoPacote, DateTime dataEntrada, DateTime dataSaida)
    {
        var diarias = (decimal)(dataSaida - dataEntrada).TotalDays;
        if (diarias <= 0) diarias = 1;

        var quarto = QuartoFactory.CriarQuarto(tipoQuarto);
        var valorBase = quarto.PrecoDiaria * diarias;

        if (!string.IsNullOrEmpty(tipoPacote))
        {
            try
            {
                var factory = PacoteFactoryProvider.ObterFactory(tipoPacote);
                var cafe = factory.CriarCafeDaManha();
                var servico = factory.CriarServico();
                
                // Café da manhã incluso em todos os dias
                valorBase += cafe.Preco * diarias;
                // Serviço adicional cobrado uma vez
                valorBase += servico.Preco;
            }
            catch
            {
                // Se pacote não existe, usa apenas valor do quarto
            }
        }

        return valorBase;
    }

    #endregion
}
