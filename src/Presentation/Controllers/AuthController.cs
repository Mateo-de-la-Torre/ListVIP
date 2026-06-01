using Application.Auth.DTOs;
using Application.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var usuario = await _authService.RegisterAsync(dto);
        return CreatedAtAction(nameof(Register), new { id = usuario.Id }, usuario);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);
        return Ok( new { message = "Login exitoso", Token = token });
    }

    [HttpPost("register/invite")]
    public async Task<IActionResult> RegisterWithInvitacion([FromBody] RegisterWithInvitacionDto dto)
    {
        var usuario = await _authService.RegisterWithInvitacionAsync(dto);
        return CreatedAtAction(nameof(RegisterWithInvitacion), new { id = usuario.Id }, usuario);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var usuario = await _authService.GetMeAsync(userId);
        if (usuario == null)
            return NotFound(new { message = "Usuario no encontrado" });
        return Ok(usuario);
    }

    [HttpPut("me/password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _authService.ChangePasswordAsync(userId, dto);
        return Ok(new { message = "Password actualizada correctamente" });
    }

}
