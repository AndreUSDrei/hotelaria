namespace PlazaHotels.Models.Interfaces;

/// <summary>
/// Interface para serviços adicionais - Abstract Product do Abstract Factory
/// </summary>
public interface IServicoAdicional
{
    string Nome { get; }
    string Descricao { get; }
    decimal Preco { get; }
    int DuracaoMinutos { get; }
}
