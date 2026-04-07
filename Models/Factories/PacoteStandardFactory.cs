using PlazaHotels.Models.Interfaces;
using PlazaHotels.Models.Quartos;
using PlazaHotels.Models.Refeicoes;
using PlazaHotels.Models.Servicos;

namespace PlazaHotels.Models.Factories;

/// <summary>
/// Concrete Factory - Pacote econômico standard
/// </summary>
public class PacoteStandardFactory : IPacoteHospedagemFactory
{
    public string NomePacote => "Pacote Standard";
    public string DescricaoPacote => "Hospedagem econômica com conforto essencial para viajantes práticos.";

    public IQuarto CriarQuarto()
    {
        return new QuartoStandard();
    }

    public IRefeicao CriarCafeDaManha()
    {
        return new CafeStandard();
    }

    public IServicoAdicional CriarServico()
    {
        return new SpaRelaxante();
    }
}
