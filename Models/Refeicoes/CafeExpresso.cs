using PlazaHotels.Models.Interfaces;

namespace PlazaHotels.Models.Refeicoes;

public class CafeExpresso : IRefeicao
{
    public string Nome => "Café da Manhã Expresso";
    public string Descricao => "Café rápido para viajantes a negócios, prático e nutritivo.";
    public decimal Preco => 25.00m;
    
    public List<string> Itens => new()
    {
        "Café forte à vontade",
        "Sucos naturais",
        "Pães e torradas",
        "Manteiga e geleia",
        "Frutas cortadas",
        "Iogurte com granola"
    };
}
