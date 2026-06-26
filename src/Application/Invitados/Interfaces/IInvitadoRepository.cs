using Application.Common.Interfaces;
using Application.Invitados.Dtos;
using Domain.Entities;

namespace Application.Invitados.Interfaces;

public interface IInvitadoRepository : IRepository<Invitado>
{
    Task<InvitadoDto?> CreateInvitadoAsync(Invitado invitado);
    Task<IEnumerable<InvitadoDto>> GetByPromotorEventoAsync(int promotorEventoId);
    Task<bool> DeleteAsync(int invitadoId, int promotorId);
    Task MarcarNoSePresentaronAsync(int eventoId);
    Task<CheckInDto?> CheckInAsync(int invitadoId, int promotorId);
}
