namespace PlazaHotels.Models.Interfaces;

/// <summary>
/// Abstract Factory - Define a criação de famílias de objetos relacionados
/// </summary>
public interface IPacoteHospedagemFactory
{
    IQuarto CriarQuarto();
    IRefeicao CriarCafeDaManha();
    IServicoAdicional CriarServico();
    string NomePacote { get; }
    string DescricaoPacote { get; }
}
