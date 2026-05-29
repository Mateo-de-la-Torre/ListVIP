using Application.Auth.DTOs;
using Application.Auth.Services;
using Microsoft.AspNetCore.Mvc;

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

}
