

using Domain.Enums;

namespace Domain.Entities
{
    public class Invitacion
    {
        public int Id { get; set; }

        public string Token { get; set; } = null!;

        public string Email { get; set; } = null!;

        public Role Role { get; set; }

        public int OrganizadorId { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool Used { get; set; } = false;

    }
}
