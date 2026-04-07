using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PlazaHotels.Models;
using PlazaHotels.Models.Factories;
using PlazaHotels.Models.Services;
using PlazaHotels.Models.ViewModels;

namespace PlazaHotels.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // Listar todos os quartos disponíveis
        var quartos = QuartoFactory.ListarTodos().ToList();
        return View(quartos);
    }

    public IActionResult Quartos()
    {
        var quartos = QuartoFactory.ListarTodos().ToList();
        return View(quartos);
    }

    public IActionResult Pacotes()
    {
        var pacotes = PacoteFactoryProvider.ListarTodas().ToList();
        return View(pacotes);
    }

    [HttpGet]
    public IActionResult Reservar()
    {
        var model = new ReservaViewModel
        {
            DataEntrada = DateTime.Today,
            DataSaida = DateTime.Today.AddDays(1)
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Reservar(ReservaViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (model.DataSaida <= model.DataEntrada)
        {
            ModelState.AddModelError("DataSaida", "Data de saída deve ser posterior à data de entrada");
            return View(model);
        }

        try
        {
            var gerenciador = GerenciadorReservas.Instancia;
            
            var reserva = gerenciador.RegistrarReserva(
                model.NomeHospede,
                model.Email,
                model.Telefone,
                model.TipoQuarto,
                model.TipoPacote ?? "",
                model.DataEntrada,
                model.DataSaida,
                model.NumeroHospedes,
                model.Observacoes
            );

            // Confirmar automaticamente a reserva
            gerenciador.ConfirmarReserva(reserva.Id);

            return RedirectToAction("Confirmacao", new { id = reserva.Id });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Confirmacao(string id)
    {
        var gerenciador = GerenciadorReservas.Instancia;
        var reserva = gerenciador.ObterReserva(id);

        if (reserva == null)
        {
            return NotFound();
        }

        return View(reserva);
    }

    [HttpGet]
    public IActionResult Disponibilidade()
    {
        var model = new DisponibilidadeViewModel
        {
            DataEntrada = DateTime.Today,
            DataSaida = DateTime.Today.AddDays(1)
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult VerificarDisponibilidade(DateTime dataEntrada, DateTime dataSaida)
    {
        var gerenciador = GerenciadorReservas.Instancia;
        var disponibilidade = gerenciador.ObterDisponibilidade(dataEntrada, dataSaida);

        var quartos = disponibilidade.Select(d =>
        {
            var quarto = QuartoFactory.CriarQuarto(d.TipoQuarto);
            return new DisponibilidadeQuartoInfo
            {
                Tipo = d.TipoQuarto,
                Descricao = quarto.Descricao,
                Disponiveis = d.QuartosDisponiveis,
                PrecoDiaria = d.PrecoDiaria,
                Comodidades = quarto.Comodidades
            };
        }).ToList();

        return Json(new { success = true, quartos });
    }

    [HttpGet]
    public IActionResult CalcularPacote(string tipoQuarto, string tipoPacote, DateTime dataEntrada, DateTime dataSaida)
    {
        try
        {
            var diarias = (int)(dataSaida - dataEntrada).TotalDays;
            if (diarias <= 0) diarias = 1;

            // Sempre usa o quarto selecionado pelo usuário
            var quarto = QuartoFactory.CriarQuarto(tipoQuarto);

            // Se não há pacote selecionado, calcula apenas o quarto
            if (string.IsNullOrEmpty(tipoPacote))
            {
                var valorTotal = quarto.PrecoDiaria * diarias;

                var resultado = new
                {
                    diarias = diarias,
                    quartoTipo = quarto.Tipo,
                    cafeNome = "Não incluso",
                    servicoNome = "Não incluso",
                    valorDiaria = quarto.PrecoDiaria,
                    valorTotal = valorTotal
                };

                return Json(new { success = true, pacote = resultado });
            }

            // Com pacote: adiciona café e serviço ao quarto escolhido
            var factory = PacoteFactoryProvider.ObterFactory(tipoPacote);
            var cafe = factory.CriarCafeDaManha();
            var servico = factory.CriarServico();

            var valorDiaria = quarto.PrecoDiaria + cafe.Preco;
            var valorTotalPacote = (valorDiaria * diarias) + servico.Preco;

            var pacote = new
            {
                diarias = diarias,
                quartoTipo = quarto.Tipo,
                cafeNome = cafe.Nome,
                servicoNome = servico.Nome,
                valorDiaria = valorDiaria,
                valorTotal = valorTotalPacote
            };

            return Json(new { success = true, pacote });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
