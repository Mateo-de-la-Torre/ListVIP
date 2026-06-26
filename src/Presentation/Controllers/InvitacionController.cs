using Application.Invitaciones.Dtos;
using Application.Invitaciones.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Organizador")]
public class InvitacionController : ControllerBase
{
    private readonly InvitacionService _invitacionService;

    public InvitacionController(InvitacionService invitacionService)
    {
        _invitacionService = invitacionService;
    }

    [HttpPost]
    public async Task<IActionResult> SendInvitacion([FromBody] SendInvitacionDto dto)
    {
        var organizadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _invitacionService.SendInvitacionAsync(dto.Email, organizadorId, dto.EventoId, dto.Commission);
        return Ok(new { message = "Invitación enviada correctamente" });
    }
}