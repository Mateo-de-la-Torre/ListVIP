

using Domain.Enums;

namespace Application.Auth.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendInvitationAsync(string email, string token, decimal? commission, string eventoName, DateTime eventoDate, string eventoLocation, decimal eventoTicketPrice);
        Task<bool> SendGuestQrAsync(string email, string guestName, string qrBase64, string eventoName, DateTime eventoDate, string eventoLocation);
    }
}
