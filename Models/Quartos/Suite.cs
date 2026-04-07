using PlazaHotels.Models.Interfaces;

namespace PlazaHotels.Models.Quartos;

/// <summary>
/// Concrete Product - Suíte presidencial com todos os serviços
/// </summary>
public class Suite : IQuarto
{
    public string Tipo => "Suíte Presidencial";
    public string Descricao => "A experiência mais exclusiva do Plaza Hotels. Suíte completa com sala de estar, jacuzzi privativo e mordomo dedicado.";
    public decimal PrecoDiaria => 999.90m;
    public int Capacidade => 4;
    public string ImagemUrl => "/images/quartos/suite.jpg";
    
    public List<string> Comodidades => new()
    {
        "Quarto principal com cama king-size",
        "Sala de estar separada com sofá-cama",
        "Segundo quarto com duas camas solteiras",
        "Banheira de hidromassagem privativa",
        "TV Smart 65\" no quarto + TV 55\" na sala",
        "Sistema de som ambiente integrado",
        "Minibar premium com champanhe",
        "Cozinha compacta equipada",
        "Varanda ampla com mobiliário de luxo",
        "Mordomo dedicado 24h",
        "Check-in/Check-out privativo",
        "Acesso exclusivo ao Lounge Executivo",
        "Transfer gratuito aeroporto/hotel",
        "Roupa de cama egípcia 600 fios"
    };
}
