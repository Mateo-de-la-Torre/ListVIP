

using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.Invitaciones.Interfaces;

public interface IInvitacionRepository : IRepository<Invitacion>
{
    Task<Invitacion?> GetByTokenAsync(string token);
    Task MarkAsUsedAsync(Invitacion invitacion);
}
