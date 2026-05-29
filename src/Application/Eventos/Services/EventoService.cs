using Application.Eventos.DTOs;
using Application.Eventos.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Eventos.Services;

public class EventoService
{
    private readonly IEventoRepository _eventoRepository;

    public EventoService(IEventoRepository eventoRepository)
    {
        _eventoRepository = eventoRepository;
    }

    public async Task<Evento> CreateAsync(CreateEventoDto dto, int organizadorId)
    {
        var evento = Evento.Create(dto.Name, dto.Date, dto.Location, dto.Capacity, dto.TicketPrice, organizadorId);
        return await _eventoRepository.CreateAsync(evento);
    }

    public async Task<IEnumerable<Evento>> GetAllAsync(int organizadorId)
    {
        return await _eventoRepository.GetAllAsync(organizadorId);
    }

    public async Task<Evento?> GetByIdAsync(int id, int organizadorId)
    {
        return await _eventoRepository.GetByIdAsync(id, organizadorId);
    }

    public async Task<Evento?> UpdateAsync(int id, UpdateEventoDto dto, int organizadorId)
    {
        var existingEvento = await _eventoRepository.GetByIdAsync(id, organizadorId);
        if (existingEvento == null) return null;
        Evento.Update(existingEvento, dto.Name, dto.Date, dto.Location, dto.Capacity, dto.TicketPrice);
        return await _eventoRepository.UpdateAsync(existingEvento);
    }

    public async Task<Evento?> ChangeStatusAsync(int id, EventStatus status, int organizadorId)
    {
        return await _eventoRepository.ChangeStatusAsync(id, status, organizadorId);
    }

    public async Task<Evento?> DeleteAsync(int id, int organizadorId)
    {
        return await _eventoRepository.DeleteAsync(id, organizadorId);
    }
}
