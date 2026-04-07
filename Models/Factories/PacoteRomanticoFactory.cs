using PlazaHotels.Models.Interfaces;
using PlazaHotels.Models.Quartos;
using PlazaHotels.Models.Refeicoes;
using PlazaHotels.Models.Servicos;

namespace PlazaHotels.Models.Factories;

/// <summary>
/// Concrete Factory - Pacote especial para casal
/// </summary>
public class PacoteRomanticoFactory : IPacoteHospedagemFactory
{
    public string NomePacote => "Pacote Romântico";
    public string DescricaoPacote => "Experiência completa para casais, com quarto luxuoso, café especial e jantar romântico.";

    public IQuarto CriarQuarto()
    {
        return new QuartoLuxo();
    }

    public IRefeicao CriarCafeDaManha()
    {
        return new CafeRomantico();
    }

    public IServicoAdicional CriarServico()
    {
        return new JantarRomantico();
    }
}
