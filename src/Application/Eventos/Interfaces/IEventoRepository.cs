using Domain.Entities;
using Domain.Enums;

namespace Application.Eventos.Interfaces;

public interface IEventoRepository
{
    Task<Evento> CreateAsync(Evento evento);

    Task<IEnumerable<Evento>> GetAllAsync(int organizadorId);

    Task<Evento?> GetByIdAsync(int id, int organizadorId);

    Task<Evento> UpdateAsync(Evento evento);

    Task<Evento?> ChangeStatusAsync(int id, EventStatus status, int organizadorId);

    Task<Evento?> DeleteAsync(int id, int organizadorId);

}
