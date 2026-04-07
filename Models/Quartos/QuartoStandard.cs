using PlazaHotels.Models.Interfaces;

namespace PlazaHotels.Models.Quartos;

/// <summary>
/// Concrete Product - Quarto básico com comodidades essenciais
/// </summary>
public class QuartoStandard : IQuarto
{
    public string Tipo => "Standard";
    public string Descricao => "Quarto aconchegante com decoração moderna, ideal para viajantes que buscam conforto e praticidade.";
    public decimal PrecoDiaria => 199.90m;
    public int Capacidade => 2;
    public string ImagemUrl => "/images/quartos/standard.jpg";
    
    public List<string> Comodidades => new()
    {
        "Cama de casal ou duas solteiras",
        "Ar-condicionado",
        "TV LED 32\" com cabo",
        "Wi-Fi gratuito",
        "Frigobar",
        "Banheiro privativo",
        "Mesa de trabalho",
        "Telefone"
    };
}
