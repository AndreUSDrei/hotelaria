using System.ComponentModel.DataAnnotations;

namespace PlazaHotels.Models.ViewModels;

public class ReservaViewModel
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 3)]
    [Display(Name = "Nome Completo")]
    public string NomeHospede { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefone é obrigatório")]
    [Phone(ErrorMessage = "Telefone inválido")]
    [Display(Name = "Telefone")]
    public string Telefone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Selecione o tipo de quarto")]
    [Display(Name = "Tipo de Quarto")]
    public string TipoQuarto { get; set; } = string.Empty;

    [Display(Name = "Pacote de Hospedagem")]
    public string? TipoPacote { get; set; }

    [Required(ErrorMessage = "Data de entrada é obrigatória")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Entrada")]
    public DateTime DataEntrada { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Data de saída é obrigatória")]
    [DataType(DataType.Date)]
    [Display(Name = "Data de Saída")]
    public DateTime DataSaida { get; set; } = DateTime.Today.AddDays(1);

    [Required(ErrorMessage = "Número de hóspedes é obrigatório")]
    [Range(1, 4, ErrorMessage = "Máximo de 4 hóspedes por quarto")]
    [Display(Name = "Número de Hóspedes")]
    public int NumeroHospedes { get; set; } = 1;

    [Display(Name = "Observações")]
    [StringLength(500)]
    public string? Observacoes { get; set; }
}

public class DisponibilidadeViewModel
{
    [DataType(DataType.Date)]
    public DateTime DataEntrada { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    public DateTime DataSaida { get; set; } = DateTime.Today.AddDays(1);

    public List<DisponibilidadeQuartoInfo>? Quartos { get; set; }
}

public class DisponibilidadeQuartoInfo
{
    public string Tipo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Disponiveis { get; set; }
    public decimal PrecoDiaria { get; set; }
    public List<string> Comodidades { get; set; } = new();
}

public class PacoteViewModel
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string QuartoTipo { get; set; } = string.Empty;
    public string CafeNome { get; set; } = string.Empty;
    public string ServicoNome { get; set; } = string.Empty;
    public decimal ValorDiaria { get; set; }
    public decimal ValorTotal { get; set; }
    public int Diarias { get; set; }
}

public class DashboardViewModel
{
    public Dictionary<string, int> Estatisticas { get; set; } = new();
    public List<ReservaInfo> ReservasAtivas { get; set; } = new();
    public List<ReservaInfo> ChegandoHoje { get; set; } = new();
    public List<ReservaInfo> SaindoHoje { get; set; } = new();
}

public class ReservaInfo
{
    public string Id { get; set; } = string.Empty;
    public string NomeHospede { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string TipoQuarto { get; set; } = string.Empty;
    public DateTime DataEntrada { get; set; }
    public DateTime DataSaida { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
}
