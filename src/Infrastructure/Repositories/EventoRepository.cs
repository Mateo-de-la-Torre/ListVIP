using Application.Eventos.Interfaces;
using Domain.Entities;
using Infrastructure.Data;

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
}
