using Application.Common.Interfaces;
using Application.Eventos.DTOs;
using Domain.Entities;
using Domain.Enums;

namespace Application.Eventos.Interfaces;

public interface IEventoRepository : IRepository<Evento>
{
    Task<EventoDto> CreateEventoAsync(Evento evento);
    Task<IEnumerable<EventoDto>> GetAllAsync(int organizadorId);
    Task<EventoDto?> GetByIdAsync(int id, int organizadorId);
    Task<EventoDto?> UpdateAsync(int id, UpdateEventoDto dto, int organizadorId);
    Task<EventoDto?> ChangeStatusAsync(int id, EventStatus status, int organizadorId);
    Task<EventoDto?> DeleteAsync(int id, int organizadorId);
}
