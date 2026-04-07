using PlazaHotels.Models.Interfaces;
using PlazaHotels.Models.Quartos;
using PlazaHotels.Models.Refeicoes;
using PlazaHotels.Models.Servicos;

namespace PlazaHotels.Models.Factories;

/// <summary>
/// Concrete Factory - Pacote premium completo
/// </summary>
public class PacotePremiumFactory : IPacoteHospedagemFactory
{
    public string NomePacote => "Pacote Premium";
    public string DescricaoPacote => "Experiência completa de luxo com suíte presidencial, café premium e spa relaxante.";

    public IQuarto CriarQuarto()
    {
        return new Suite();
    }

    public IRefeicao CriarCafeDaManha()
    {
        return new CafePremium();
    }

    public IServicoAdicional CriarServico()
    {
        return new SpaRelaxante();
    }
}
