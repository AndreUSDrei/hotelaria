namespace PlazaHotels.Models.Interfaces;

/// <summary>
/// Interface base para qualquer tipo de quarto - Product do Factory Method
/// </summary>
public interface IQuarto
{
    string Tipo { get; }
    string Descricao { get; }
    decimal PrecoDiaria { get; }
    int Capacidade { get; }
    List<string> Comodidades { get; }
    string ImagemUrl { get; }
}
