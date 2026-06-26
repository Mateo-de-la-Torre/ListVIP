using Application.Eventos.DTOs;
using Application.Eventos.Interfaces;
using Application.Invitados.Interfaces;
using Application.PromotorEventos.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Eventos.Services;

public class EventoService
{
    private readonly IEventoRepository _eventoRepository;
    private readonly IPromotorEventoRepository _promotorEventoRepository;
    private readonly IInvitadoRepository _invitadoRepository;

    public EventoService(IEventoRepository eventoRepository, IPromotorEventoRepository promotorEventoRepository, IInvitadoRepository invitadoRepository)
    {
        _eventoRepository = eventoRepository;
        _promotorEventoRepository = promotorEventoRepository;
        _invitadoRepository = invitadoRepository;
    }

    public async Task<EventoDto> CreateAsync(CreateEventoDto dto, int organizadorId)
    {
        var evento = Evento.Create(dto.Name, dto.Date, dto.Location, dto.Capacity, dto.TicketPrice, organizadorId);
        return await _eventoRepository.CreateEventoAsync(evento);
    }

    public async Task<IEnumerable<EventoDto>> GetAllAsync(int organizadorId)
    {
        return await _eventoRepository.GetAllAsync(organizadorId);
    }

    public async Task<EventoDto?> GetByIdAsync(int id, int organizadorId)
    {
        return await _eventoRepository.GetByIdAsync(id, organizadorId);
    }

    public async Task<EventoDto?> UpdateAsync(int id, UpdateEventoDto dto, int organizadorId)
    {
        return await _eventoRepository.UpdateAsync(id, dto, organizadorId);
    }

    public async Task<EventoDto?> ChangeStatusAsync(int id, EventStatus status, int organizadorId)
    {
        var dto = await _eventoRepository.ChangeStatusAsync(id, status, organizadorId);

        if (dto != null && status == EventStatus.Finalizado)
        {
            await _promotorEventoRepository.CalcularComisionesAsync(dto.Id, dto.TicketPrice);
            await _invitadoRepository.MarcarNoSePresentaronAsync(dto.Id);
        }

        return dto;
    }

    public async Task<EventoDto?> DeleteAsync(int id, int organizadorId)
    {
        return await _eventoRepository.DeleteAsync(id, organizadorId);
    }
}
