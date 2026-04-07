using PlazaHotels.Models.Interfaces;
using PlazaHotels.Models.Quartos;

namespace PlazaHotels.Models.Factories;

/// <summary>
/// Factory Method - Fábrica para criação de tipos de quarto
/// </summary>
public static class QuartoFactory
{
    public static IQuarto CriarQuarto(string tipo)
    {
        return tipo?.ToLower() switch
        {
            "standard" => new QuartoStandard(),
            "luxo" => new QuartoLuxo(),
            "suite" or "suíte" or "suíte presidencial" => new Suite(),
            _ => throw new ArgumentException($"Tipo de quarto '{tipo}' não reconhecido. Tipos válidos: Standard, Luxo, Suite")
        };
    }

    public static IEnumerable<IQuarto> ListarTodos()
    {
        return new List<IQuarto>
        {
            new QuartoStandard(),
            new QuartoLuxo(),
            new Suite()
        };
    }

    public static IEnumerable<string> TiposDisponiveis()
    {
        return new List<string> { "Standard", "Luxo", "Suite" };
    }
}
