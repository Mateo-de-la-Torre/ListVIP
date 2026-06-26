namespace Application.Invitados.Dtos;

public class CreateInvitadoDto
{
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
}
