using PlazaHotels.Models.Interfaces;

namespace PlazaHotels.Models.Refeicoes;

public class CafePremium : IRefeicao
{
    public string Nome => "Café da Manhã Premium";
    public string Descricao => "Experiência gastronômica completa com produtos selecionados e pratos quentes.";
    public decimal Preco => 65.00m;
    
    public List<string> Itens => new()
    {
        "Café especialidade (arabica, blend exclusivo)",
        "Leite integral, desnatado e vegetal",
        "Sucos naturais variados",
        "Buffet de pães artesanais",
        "Manteiga gourmet e geleias importadas",
        "Frutas tropicais selecionadas",
        "Ovos preparados na hora",
        "Bacon e linguiças grelhadas",
        "Panquecas e waffles",
        "Croissants e brioches",
        "Queijos e friados",
        "Cereais, granola e iogurte grego"
    };
}
