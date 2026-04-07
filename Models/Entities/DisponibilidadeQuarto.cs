namespace PlazaHotels.Models.Entities;

public class DisponibilidadeQuarto
{
    public string TipoQuarto { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public int TotalQuartos { get; set; }
    public int QuartosOcupados { get; set; }
    public int QuartosDisponiveis => TotalQuartos - QuartosOcupados;
    public decimal PrecoDiaria { get; set; }
}
