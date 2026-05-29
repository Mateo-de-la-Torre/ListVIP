using Application.Eventos.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EventoRepository : IEventoRepository
{
    private readonly ListVIPContext _context;

    public EventoRepository(ListVIPContext context)
    {
        _context = context;
    }

    public async Task<Evento> CreateAsync(Evento evento)
    {
        _context.Eventos.Add(evento);
        await _context.SaveChangesAsync();
        return evento;
    }

    public async Task<IEnumerable<Evento>> GetAllAsync(int organizadorId)
    {
        return await _context.Eventos.Where(e => e.OrganizadorId == organizadorId).ToListAsync();
    } 

    public async Task<Evento?> GetByIdAsync(int id, int organizadorId)
    {
        return await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id && e.OrganizadorId == organizadorId);
    }

    public async Task<Evento> UpdateAsync(Evento evento)
    {
        _context.Eventos.Update(evento);
        await _context.SaveChangesAsync();
        return evento;
    }

    public async Task<Evento?> ChangeStatusAsync(int id, EventStatus status, int organizadorId)
    {
        var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id && e.OrganizadorId == organizadorId);
        if (evento == null)
            return null;
        evento.EventStatus = status;
        await _context.SaveChangesAsync();
        return evento;
    }

    public async Task<Evento?> DeleteAsync(int id, int organizadorId)
    {
        var evento = await _context.Eventos.FirstOrDefaultAsync(e => e.Id == id && e.OrganizadorId == organizadorId);
        if (evento == null)
            return null;
        evento.Active = false;
        await _context.SaveChangesAsync();
        return evento;
    }
}
