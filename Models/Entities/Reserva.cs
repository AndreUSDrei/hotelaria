namespace PlazaHotels.Models.Entities;

public class Reserva
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string NomeHospede { get; set; } = string.Empty;
    public string EmailHospede { get; set; } = string.Empty;
    public string TelefoneHospede { get; set; } = string.Empty;
    public string TipoQuarto { get; set; } = string.Empty;
    public string TipoPacote { get; set; } = string.Empty;
    public DateTime DataEntrada { get; set; }
    public DateTime DataSaida { get; set; }
    public int NumeroHospedes { get; set; }
    public decimal ValorTotal { get; set; }
    public StatusReserva Status { get; set; } = StatusReserva.Pendente;
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public string? Observacoes { get; set; }
    public bool CafeDaManhaIncluso { get; set; } = true;
    public bool ServicoAdicionalIncluso { get; set; } = false;
}

public enum StatusReserva
{
    Pendente,
    Confirmada,
    CheckIn,
    CheckOut,
    Cancelada
}
