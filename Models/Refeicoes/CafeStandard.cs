using PlazaHotels.Models.Interfaces;

namespace PlazaHotels.Models.Refeicoes;

public class CafeStandard : IRefeicao
{
    public string Nome => "Café da Manhã Standard";
    public string Descricao => "Café da manhã completo com opções variadas para começar o dia.";
    public decimal Preco => 35.00m;
    
    public List<string> Itens => new()
    {
        "Café premium à vontade",
        "Leite integral e desnatado",
        "Sucos naturais (laranja, maracujá)",
        "Pães variados (francês, integral)",
        "Manteiga e geleias",
        "Frutas da estação",
        "Bolos caseiros",
        "Cereais e iogurte"
    };
}
