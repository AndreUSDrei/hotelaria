using PlazaHotels.Models.Interfaces;

namespace PlazaHotels.Models.Servicos;

public class JantarRomantico : IServicoAdicional
{
    public string Nome => "Jantar Romântico";
    public string Descricao => "Jantar a luz de velas com menu degustação para casais.";
    public decimal Preco => 350.00m;
    public int DuracaoMinutos => 180;
}
