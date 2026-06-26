using Domain.Enums;

namespace Application.Invitados.Dtos;

public class InvitadoDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public GuestStatus GuestStatus { get; set; }
}
