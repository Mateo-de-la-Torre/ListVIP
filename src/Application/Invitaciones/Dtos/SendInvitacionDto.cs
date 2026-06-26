namespace Application.Invitaciones.Dtos
{
    public class SendInvitacionDto
    {
        public string Email { get; set; } = null!;
        public int EventoId { get; set; }
        public decimal? Commission { get; set; }
    }
}
