using Domain.Enums;

namespace Application.Invitados.Dtos;

public class CheckInDto
{
    public int InvitadoId { get; set; }
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public GuestStatus GuestStatus { get; set; }
    public DateTime AccessTime { get; set; }
}
