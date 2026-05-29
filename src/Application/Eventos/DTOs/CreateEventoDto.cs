namespace Application.Eventos.DTOs;

public class CreateEventoDto
{
    public string Name { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Location { get; set; } = null!;
    public int Capacity { get; set; }
    public decimal TicketPrice { get; set; }
    public int OrganizadorId { get; set; }
}
