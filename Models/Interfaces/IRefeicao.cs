namespace PlazaHotels.Models.Interfaces;

/// <summary>
/// Interface para refeições - Abstract Product do Abstract Factory
/// </summary>
public interface IRefeicao
{
    string Nome { get; }
    string Descricao { get; }
    decimal Preco { get; }
    List<string> Itens { get; }
}
