using Domain.Entities;

namespace Application.Eventos.Interfaces;

public interface IEventoRepository
{
    Task<Evento> CreateAsync(Evento evento);

}
