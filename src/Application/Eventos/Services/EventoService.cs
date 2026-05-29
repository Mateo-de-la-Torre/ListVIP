using Application.Eventos.DTOs;
using Application.Eventos.Interfaces;
using Domain.Entities;

namespace Application.Eventos.Services;

public class EventoService
{
    private readonly IEventoRepository _eventoRepository;

    public EventoService(IEventoRepository eventoRepository)
    {
        _eventoRepository = eventoRepository;
    }

    public async Task<Evento> CreateAsync(CreateEventoDto dto)
    {
        var evento = Evento.Create(dto.Name, dto.Date, dto.Location, dto.Capacity, dto.TicketPrice, dto.OrganizadorId);
        return await _eventoRepository.CreateAsync(evento);
    }
}
