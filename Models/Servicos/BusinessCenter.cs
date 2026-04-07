using PlazaHotels.Models.Interfaces;

namespace PlazaHotels.Models.Servicos;

public class BusinessCenter : IServicoAdicional
{
    public string Nome => "Business Center";
    public string Descricao => "Acesso ao centro de negócios com computadores, impressora e secretária.";
    public decimal Preco => 80.00m;
    public int DuracaoMinutos => 480;
}
