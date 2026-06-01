using Domain.Enums;

namespace Application.Invitaciones.Dtos
{
    public class SendInvitacionDto
    {
        public string Email { get; set; } = null!;
        public Role Role { get; set; }
    }
}
