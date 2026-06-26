using Application.PromotorEventos.Dtos;
using Application.PromotorEventos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers;

[ApiController]
[Route("api/events/{eventoId}/promoters")]
public class PromotorEventoController : ControllerBase
{
    private readonly PromotorEventoService _promotorEventoService;

    public PromotorEventoController(PromotorEventoService promotorEventoService)
    {
        _promotorEventoService = promotorEventoService;
    }

    [HttpGet("/api/promoter/events")]
    [Authorize(Roles = "Promotor")]
    public async Task<IActionResult> GetMisEventos()
    {
        var promotorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _promotorEventoService.GetEventosByPromotorAsync(promotorId);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = "Organizador")]
    public async Task<IActionResult> GetPromotoresByEvento(int eventoId)
    {
        var organizadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var promotores = await _promotorEventoService.GetByEventoIdAsync(eventoId, organizadorId);
        return Ok(promotores);
    }

    [HttpPost]
    [Authorize(Roles = "Organizador")]
    public async Task<IActionResult> AsignarPromotor(int eventoId, [FromBody] AsignarPromotorDto dto)
    {
        var organizadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _promotorEventoService.AsignarPromotorAsync(eventoId, dto.PromotorId, dto.Commission, organizadorId);
        return CreatedAtAction(nameof(GetPromotoresByEvento), new { eventoId }, result);
    }

    [HttpPatch("{promotorId}/accept")]
    [Authorize(Roles = "Promotor")]
    public async Task<IActionResult> AceptarAsignacion(int eventoId, int promotorId)
    {
        var promotorJwtId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if (promotorJwtId != promotorId)
            return Forbid();

        var result = await _promotorEventoService.AceptarAsync(eventoId, promotorId);
        return Ok(result);
    }

    [HttpPatch("{promotorId}/reject")]
    [Authorize(Roles = "Promotor")]
    public async Task<IActionResult> RechazarAsignacion(int eventoId, int promotorId)
    {
        var promotorJwtId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        if (promotorJwtId != promotorId)
            return Forbid();

        var result = await _promotorEventoService.RechazarAsync(eventoId, promotorId);
        return Ok(result);
    }

    [HttpPatch("{promotorId}/settle")]
    [Authorize(Roles = "Organizador")]
    public async Task<IActionResult> LiquidarComision(int eventoId, int promotorId)
    {
        var organizadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _promotorEventoService.LiquidarAsync(eventoId, promotorId, organizadorId);
        return Ok(result);
    }
}
