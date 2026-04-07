using PlazaHotels.Models.Interfaces;

namespace PlazaHotels.Models.Servicos;

public class SpaRelaxante : IServicoAdicional
{
    public string Nome => "Spa Relaxante";
    public string Descricao => "Sessão de spa completa com massagem, sauna e banho de ofurô.";
    public decimal Preco => 180.00m;
    public int DuracaoMinutos => 120;
}
