using Domain.Enums;

namespace Application.Eventos.DTOs;

public class EventoDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Location { get; set; } = null!;
    public int Capacity { get; set; }
    public decimal TicketPrice { get; set; }
    public EventStatus EventStatus { get; set; }
}
