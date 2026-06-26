using Application.Eventos.DTOs;
using Application.Eventos.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EventoRepository : Repository<Evento>, IEventoRepository
{
    public EventoRepository(ListVIPContext context) : base(context) { }

    public async Task<EventoDto> CreateEventoAsync(Evento evento)
    {
        _context.Eventos.Add(evento);
        await _context.SaveChangesAsync();
        return ToDto(evento);
    }

    public async Task<IEnumerable<EventoDto>> GetAllAsync(int organizadorId)
    {
        return await _context.Eventos
            .Where(e => e.OrganizadorId == organizadorId && e.Active)
            .Select(e => ToDto(e))
            .ToListAsync();
    }

    public async Task<EventoDto?> GetByIdAsync(int id, int organizadorId)
    {
        var e = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id && e.OrganizadorId == organizadorId);
        return e == null ? null : ToDto(e);
    }

    public async Task<EventoDto?> UpdateAsync(int id, UpdateEventoDto dto, int organizadorId)
    {
        var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id && e.OrganizadorId == organizadorId && e.Active);
        if (evento == null) return null;
        Evento.Update(evento, dto.Name, dto.Date, dto.Location, dto.Capacity, dto.TicketPrice);
        await _context.SaveChangesAsync();
        return ToDto(evento);
    }

    public async Task<EventoDto?> ChangeStatusAsync(int id, EventStatus status, int organizadorId)
    {
        var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id && e.OrganizadorId == organizadorId);
        if (evento == null) return null;
        evento.EventStatus = status;
        await _context.SaveChangesAsync();
        return ToDto(evento);
    }

    public async Task<EventoDto?> DeleteAsync(int id, int organizadorId)
    {
        var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id && e.OrganizadorId == organizadorId);
        if (evento == null) return null;
        evento.Active = false;
        await _context.SaveChangesAsync();
        return ToDto(evento);
    }

    private static EventoDto ToDto(Evento e) => new()
    {
        Id = e.Id,
        Name = e.Name,
        Date = e.Date,
        Location = e.Location,
        Capacity = e.Capacity,
        TicketPrice = e.TicketPrice,
        EventStatus = e.EventStatus
    };
}
