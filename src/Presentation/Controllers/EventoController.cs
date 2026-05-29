using Application.Eventos.DTOs;
using Application.Eventos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        var organizadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var evento = await _eventoService.CreateAsync(dto, organizadorId);
        return CreatedAtAction(nameof(Create), new { id = evento.Id }, evento);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var organizadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var eventos = await _eventoService.GetAllAsync(organizadorId);
        return Ok(eventos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var organizadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var evento = await _eventoService.GetByIdAsync(id, organizadorId);
        if (evento == null)
            return NotFound(new { message = "Evento no encontrado" });
        return Ok(evento);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEventoDto dto)
    {
        var organizadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var evento = await _eventoService.UpdateAsync(id, dto, organizadorId);
        if (evento == null)
            return NotFound(new { message = "Evento no encontrado" });
        return Ok(evento);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeStatusDto dto)
    {
        var organizadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var evento = await _eventoService.ChangeStatusAsync(id, dto.Status, organizadorId);
        if (evento == null)
            return NotFound(new { message = "Evento no encontrado" });
        return Ok(evento);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var organizadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var evento = await _eventoService.DeleteAsync(id, organizadorId);
        if (evento == null)
            return NotFound(new { message = "Evento no encontrado" });
        return Ok(new { message = "Evento eliminado correctamente", evento });
    }
}
