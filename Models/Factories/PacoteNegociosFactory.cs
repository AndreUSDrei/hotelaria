using PlazaHotels.Models.Interfaces;
using PlazaHotels.Models.Quartos;
using PlazaHotels.Models.Refeicoes;
using PlazaHotels.Models.Servicos;

namespace PlazaHotels.Models.Factories;

/// <summary>
/// Concrete Factory - Pacote para viajante corporativo
/// </summary>
public class PacoteNegociosFactory : IPacoteHospedagemFactory
{
    public string NomePacote => "Pacote Negócios";
    public string DescricaoPacote => "Ideal para viagens corporativas, com infraestrutura completa para reuniões e trabalho.";

    public IQuarto CriarQuarto()
    {
        return new QuartoStandard();
    }

    public IRefeicao CriarCafeDaManha()
    {
        return new CafeExpresso();
    }

    public IServicoAdicional CriarServico()
    {
        return new BusinessCenter();
    }
}
