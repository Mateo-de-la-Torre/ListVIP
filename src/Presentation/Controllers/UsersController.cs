using Application.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Organizador")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPromotores()
    {
        var organizadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var promotores = await _userService.GetPromotoresByOrganizadorAsync(organizadorId);
        return Ok(promotores);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var organizadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var promotor = await _userService.ToggleActiveAsync(id, organizadorId);
        if (promotor == null)
            return NotFound(new { message = "Promotor no encontrado." });
        return Ok(new { promotor.Id, promotor.Email, promotor.Active });
    }
}
