using PlazaHotels.Models.Interfaces;

namespace PlazaHotels.Models.Quartos;

/// <summary>
/// Concrete Product - Quarto com vista e amenidades premium
/// </summary>
public class QuartoLuxo : IQuarto
{
    public string Tipo => "Luxo";
    public string Descricao => "Quarto espaçoso com vista panorâmica, decoração sofisticada e amenidades premium para uma estadia inesquecível.";
    public decimal PrecoDiaria => 399.90m;
    public int Capacidade => 2;
    public string ImagemUrl => "/images/quartos/luxo.jpg";
    
    public List<string> Comodidades => new()
    {
        "Cama king-size premium",
        "Ar-condicionado split silencioso",
        "TV Smart 55\" com Netflix/YouTube",
        "Wi-Fi de alta velocidade",
        "Minibar completo",
        "Banheiro com banheira de hidromassagem",
        "Varanda com vista panorâmica",
        "Roupa de cama premium 400 fios",
        "Fechadura digital",
        "Room service 24h"
    };
}
