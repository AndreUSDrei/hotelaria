using PlazaHotels.Models.Interfaces;

namespace PlazaHotels.Models.Factories;

/// <summary>
/// Provider para obter a fábrica de pacotes correta
/// </summary>
public static class PacoteFactoryProvider
{
    public static IPacoteHospedagemFactory ObterFactory(string tipoPacote)
    {
        return tipoPacote?.ToLower() switch
        {
            "romantico" or "romântico" => new PacoteRomanticoFactory(),
            "negocios" or "negócios" => new PacoteNegociosFactory(),
            "premium" => new PacotePremiumFactory(),
            "standard" => new PacoteStandardFactory(),
            _ => throw new ArgumentException($"Pacote '{tipoPacote}' não reconhecido. Pacotes válidos: Romantico, Negocios, Premium, Standard")
        };
    }

    public static IEnumerable<IPacoteHospedagemFactory> ListarTodas()
    {
        return new List<IPacoteHospedagemFactory>
        {
            new PacoteStandardFactory(),
            new PacoteNegociosFactory(),
            new PacoteRomanticoFactory(),
            new PacotePremiumFactory()
        };
    }

    public static IEnumerable<string> PacotesDisponiveis()
    {
        return new List<string> { "Standard", "Negocios", "Romantico", "Premium" };
    }
}
