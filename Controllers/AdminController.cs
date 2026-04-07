using Microsoft.AspNetCore.Mvc;
using PlazaHotels.Models.Entities;
using PlazaHotels.Models.Services;
using PlazaHotels.Models.ViewModels;

namespace PlazaHotels.Controllers;

/// <summary>
/// Controller responsável pela área administrativa, gerenciando o dashboard, 
/// visualização de reservas e operações de Check-in/Check-out.
/// </summary>
public class AdminController : Controller
{
    /// <summary>
    /// Renderiza o Dashboard principal com estatísticas, chegadas e saídas do dia.
    /// </summary>
    public IActionResult Index()
    {
        var gerenciador = GerenciadorReservas.Instancia;
        var estatisticas = gerenciador.ObterEstatisticas();
        var reservasAtivas = gerenciador.ListarReservasAtivas();

        var hoje = DateTime.Today;

        // Monta o ViewModel consolidando os dados para a View
        var model = new DashboardViewModel
        {
            Estatisticas = estatisticas,
            // Lista geral de reservas ativas
            ReservasAtivas = reservasAtivas.Select(r => new ReservaInfo
            {
                Id = r.Id,
                NomeHospede = r.NomeHospede,
                Email = r.EmailHospede,
                TipoQuarto = r.TipoQuarto,
                DataEntrada = r.DataEntrada,
                DataSaida = r.DataSaida,
                Status = r.Status.ToString(),
                ValorTotal = r.ValorTotal
            }).ToList(),
            
            // Filtra hóspedes que devem chegar (Check-in) hoje
            ChegandoHoje = reservasAtivas
                .Where(r => r.DataEntrada.Date == hoje && r.Status == StatusReserva.Confirmada)
                .Select(r => new ReservaInfo
                {
                    Id = r.Id,
                    NomeHospede = r.NomeHospede,
                    Email = r.EmailHospede,
                    TipoQuarto = r.TipoQuarto,
                    DataEntrada = r.DataEntrada,
                    DataSaida = r.DataSaida,
                    Status = r.Status.ToString(),
                    ValorTotal = r.ValorTotal
                }).ToList(),
            
            // Filtra hóspedes que devem sair (Check-out) hoje
            SaindoHoje = reservasAtivas
                .Where(r => r.DataSaida.Date == hoje && r.Status == StatusReserva.CheckIn)
                .Select(r => new ReservaInfo
                {
                    Id = r.Id,
                    NomeHospede = r.NomeHospede,
                    Email = r.EmailHospede,
                    TipoQuarto = r.TipoQuarto,
                    DataEntrada = r.DataEntrada,
                    DataSaida = r.DataSaida,
                    Status = r.Status.ToString(),
                    ValorTotal = r.ValorTotal
                }).ToList()
        };

        return View(model);
    }

    /// <summary>
    /// Lista todas as reservas que não foram finalizadas ou canceladas.
    /// </summary>
    public IActionResult Reservas()
    {
        var gerenciador = GerenciadorReservas.Instancia;
        var reservas = gerenciador.ListarReservasAtivas();

        var model = reservas.Select(r => new ReservaInfo
        {
            Id = r.Id,
            NomeHospede = r.NomeHospede,
            Email = r.EmailHospede,
            TipoQuarto = r.TipoQuarto,
            DataEntrada = r.DataEntrada,
            DataSaida = r.DataSaida,
            Status = r.Status.ToString(),
            ValorTotal = r.ValorTotal
        }).ToList();

        return View(model);
    }

    /// <summary>
    /// Exibe o histórico completo de reservas (incluindo finalizadas e canceladas).
    /// </summary>
    public IActionResult Historico()
    {
        var gerenciador = GerenciadorReservas.Instancia;
        var historico = gerenciador.ListarHistorico();

        var model = historico.Select(r => new ReservaInfo
        {
            Id = r.Id,
            NomeHospede = r.NomeHospede,
            Email = r.EmailHospede,
            TipoQuarto = r.TipoQuarto,
            DataEntrada = r.DataEntrada,
            DataSaida = r.DataSaida,
            Status = r.Status.ToString(),
            ValorTotal = r.ValorTotal
        }).ToList();

        return View(model);
    }

    /// <summary>
    /// Processa a confirmação de entrada (Check-in) de um hóspede via AJAX.
    /// </summary>
    [HttpPost]
    public IActionResult CheckIn(string id)
    {
        var gerenciador = GerenciadorReservas.Instancia;
        
        try
        {
            var sucesso = gerenciador.RealizarCheckIn(id);
            return Json(new { success = sucesso });
        }
        catch (InvalidOperationException ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Processa a saída (Check-out) de um hóspede e libera o quarto via AJAX.
    /// </summary>
    [HttpPost]
    public IActionResult CheckOut(string id)
    {
        var gerenciador = GerenciadorReservas.Instancia;
        
        try
        {
            var sucesso = gerenciador.RealizarCheckOut(id);
            return Json(new { success = sucesso });
        }
        catch (InvalidOperationException ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }

    /// <summary>
    /// Cancela uma reserva pendente ou confirmada.
    /// </summary>
    [HttpPost]
    public IActionResult Cancelar(string id)
    {
        var gerenciador = GerenciadorReservas.Instancia;
        var sucesso = gerenciador.CancelarReserva(id);
        return Json(new { success = sucesso });
    }

    /// <summary>
    /// Exibe os dados detalhados de uma reserva específica.
    /// </summary>
    [HttpGet]
    public IActionResult DetalhesReserva(string id)
    {
        var gerenciador = GerenciadorReservas.Instancia;
        var reserva = gerenciador.ObterReserva(id);

        if (reserva == null)
        {
            return NotFound();
        }

        return View(reserva);
    }
}