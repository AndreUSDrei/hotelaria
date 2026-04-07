using PlazaHotels.Models.Interfaces;

namespace PlazaHotels.Models.Refeicoes;

public class CafeRomantico : IRefeicao
{
    public string Nome => "Café da Manhã Romântico";
    public string Descricao => "Café da manhã especial para casais, servido no quarto com decoração exclusiva.";
    public decimal Preco => 120.00m;
    
    public List<string> Itens => new()
    {
        "Champanhe ou espumante",
        "Café especialidade servido em taças",
        "Frutas vermelhas selecionadas",
        "Croissants de chocolate",
        "Bruschettas doces",
        "Torta de frutas da estação",
        "Queijos finos",
        "Mel orgânico com nozes",
        "Flores frescas na decoração"
    };
}
