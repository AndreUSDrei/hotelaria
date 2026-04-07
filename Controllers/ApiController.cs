using Microsoft.AspNetCore.Mvc;
using PlazaHotels.Models.Factories;
using PlazaHotels.Models.Services;

namespace PlazaHotels.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApiController : ControllerBase
{
    /// <summary>
    /// Retorna a lista de todos os tipos de quartos disponíveis e suas características.
    /// </summary>
    [HttpGet("quartos")]
    public IActionResult GetQuartos()
    {
        var quartos = QuartoFactory.ListarTodos();
        return Ok(quartos.Select(q => new
        {
            q.Tipo,
            q.Descricao,
            q.PrecoDiaria,
            q.Capacidade,
            Comodidades = q.Comodidades
        }));
    }

    /// <summary>
    /// Retorna os pacotes promocionais, detalhando o quarto, café e serviços inclusos.
    /// </summary>
    [HttpGet("pacotes")]
    public IActionResult GetPacotes()
    {
        var pacotes = PacoteFactoryProvider.ListarTodas();
        return Ok(pacotes.Select(p => new
        {
            p.NomePacote,
            p.DescricaoPacote,
            Quarto = p.CriarQuarto().Tipo,
            Cafe = p.CriarCafeDaManha().Nome,
            Servico = p.CriarServico().Nome
        }));
    }

    /// <summary>
    /// Consulta a ocupação e disponibilidade de quartos para um período específico.
    /// </summary>
    [HttpGet("disponibilidade")]
    public IActionResult GetDisponibilidade(DateTime dataEntrada, DateTime dataSaida)
    {
        var gerenciador = GerenciadorReservas.Instancia;
        var disponibilidade = gerenciador.ObterDisponibilidade(dataEntrada, dataSaida);

        return Ok(disponibilidade.Select(d => new
        {
            d.TipoQuarto,
            d.TotalQuartos,
            d.QuartosOcupados,
            d.QuartosDisponiveis,
            d.PrecoDiaria
        }));
    }

    /// <summary>
    /// Lista todas as reservas que estão atualmente ativas no sistema.
    /// </summary>
    [HttpGet("reservas")]
    public IActionResult GetReservas()
    {
        var gerenciador = GerenciadorReservas.Instancia;
        var reservas = gerenciador.ListarReservasAtivas();

        return Ok(reservas.Select(r => new
        {
            r.Id,
            r.NomeHospede,
            r.EmailHospede,
            r.TipoQuarto,
            r.TipoPacote,
            r.DataEntrada,
            r.DataSaida,
            r.NumeroHospedes,
            r.ValorTotal,
            Status = r.Status.ToString()
        }));
    }

    /// <summary>
    /// Obtém dados estatísticos de desempenho do hotel (taxa de ocupação, faturamento, etc).
    /// </summary>
    [HttpGet("estatisticas")]
    public IActionResult GetEstatisticas()
    {
        var gerenciador = GerenciadorReservas.Instancia;
        return Ok(gerenciador.ObterEstatisticas());
    }

    /// <summary>
    /// Realiza o registro e a confirmação imediata de uma nova reserva.
    /// </summary>
    [HttpPost("reservar")]
    public IActionResult CriarReserva([FromBody] ReservaRequest request)
    {
        try
        {
            var gerenciador = GerenciadorReservas.Instancia;
            
            // Registra a intenção de reserva com os dados do hóspede
            var reserva = gerenciador.RegistrarReserva(
                request.NomeHospede,
                request.Email,
                request.Telefone,
                request.TipoQuarto,
                request.TipoPacote ?? "",
                request.DataEntrada,
                request.DataSaida,
                request.NumeroHospedes,
                request.Observacoes
            );

            // Altera o status da reserva para confirmado
            gerenciador.ConfirmarReserva(reserva.Id);

            return Ok(new
            {
                success = true,
                reservaId = reserva.Id,
                valorTotal = reserva.ValorTotal
            });
        }
        catch (InvalidOperationException ex)
        {
            // Retorna erro caso não haja disponibilidade ou dados inválidos
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Registra a entrada do hóspede no hotel.
    /// </summary>
    [HttpPost("checkin/{id}")]
    public IActionResult CheckIn(string id)
    {
        var gerenciador = GerenciadorReservas.Instancia;
        
        try
        {
            var sucesso = gerenciador.RealizarCheckIn(id);
            return Ok(new { success = sucesso });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Registra a saída do hóspede e encerra a conta da reserva.
    /// </summary>
    [HttpPost("checkout/{id}")]
    public IActionResult CheckOut(string id)
    {
        var gerenciador = GerenciadorReservas.Instancia;
        
        try
        {
            var sucesso = gerenciador.RealizarCheckOut(id);
            return Ok(new { success = sucesso });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, error = ex.Message });
        }
    }
}

/// <summary>
/// Objeto de transferência de dados (DTO) para criação de reservas.
/// </summary>
public class ReservaRequest
{
    public string NomeHospede { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string TipoQuarto { get; set; } = string.Empty;
    public string? TipoPacote { get; set; }
    public DateTime DataEntrada { get; set; }
    public DateTime DataSaida { get; set; }
    public int NumeroHospedes { get; set; } = 1;
    public string? Observacoes { get; set; }
}
