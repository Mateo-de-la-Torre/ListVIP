

namespace Application.Eventos.DTOs;

public class UpdateEventoDto
{
    public string? Name { get; set; }
    public DateTime? Date { get; set; }
    public string? Location { get; set; }
    public int? Capacity { get; set; }
    public decimal? TicketPrice { get; set; }
}

