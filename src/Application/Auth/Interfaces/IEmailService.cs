

using Domain.Enums;

namespace Application.Auth.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendInvitationAsync(string email, string token, Role role);
    }
}
