using Application.Eventos.DTOs;
using Application.Eventos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventoController : ControllerBase
{
    private readonly EventoService _eventoService;

    public EventoController(EventoService eventoService)
    {
        _eventoService = eventoService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateEventoDto dto)
    {
        var evento = await _eventoService.CreateAsync(dto);
        return CreatedAtAction(nameof(Create), new { id = evento.Id }, evento);
    }
}
