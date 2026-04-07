using PlazaHotels.Models.Interfaces;

namespace PlazaHotels.Models.Servicos;

public class SalaReuniao : IServicoAdicional
{
    public string Nome => "Sala de Reunião";
    public string Descricao => "Sala executiva equipada para reuniões de negócios.";
    public decimal Preco => 150.00m;
    public int DuracaoMinutos => 240;
}
