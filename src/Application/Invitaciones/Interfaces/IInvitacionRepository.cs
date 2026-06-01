

using Domain.Entities;

namespace Application.Invitaciones.Interfaces
{
    public interface IInvitacionRepository
    {
        Task<Invitacion?> GetByTokenAsync(string token); Task CreateAsync(Invitacion invitacion);

        Task MarkAsUsedAsync(Invitacion invitacion);
    }
}
