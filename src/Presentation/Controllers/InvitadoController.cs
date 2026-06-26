using Application.Invitados.Dtos;
using Application.Invitados.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers;

[ApiController]
[Route("api/events/{eventoId}/guests")]
[Authorize(Roles = "Promotor")]
public class InvitadoController : ControllerBase
{
    private readonly InvitadoService _invitadoService;

    public InvitadoController(InvitadoService invitadoService)
    {
        _invitadoService = invitadoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetGuests(int eventoId)
    {
        var promotorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _invitadoService.GetGuestsAsync(eventoId, promotorId);
        return Ok(result);
    }

    [HttpPatch("/api/guests/{invitadoId}/checkin")]
    public async Task<IActionResult> CheckIn(int invitadoId)
    {
        var promotorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _invitadoService.CheckInAsync(invitadoId, promotorId);
        return Ok(result);
    }

    [HttpDelete("/api/guests/{invitadoId}")]
    public async Task<IActionResult> DeleteGuest(int invitadoId)
    {
        var promotorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _invitadoService.DeleteGuestAsync(invitadoId, promotorId);
        return Ok(new { message = "Invitado eliminado correctamente." });
    }

    [HttpPost]
    public async Task<IActionResult> AddGuest(int eventoId, [FromBody] CreateInvitadoDto dto)
    {
        var promotorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var result = await _invitadoService.AddGuestAsync(eventoId, promotorId, dto);
        return Created(string.Empty, result);
    }
}
