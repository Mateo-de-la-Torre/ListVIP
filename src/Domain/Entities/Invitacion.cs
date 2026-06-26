namespace Domain.Entities
{
    public class Invitacion
    {
        public int Id { get; set; }
        public int EventoId { get; set; }
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int OrganizadorId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool Used { get; set; } = false;
        public decimal? Commission { get; set; }
    }
}
