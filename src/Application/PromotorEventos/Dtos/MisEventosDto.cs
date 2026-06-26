using Domain.Enums;

namespace Application.PromotorEventos.Dtos;

public class MisEventosDto
{
    public int PromotorEventoId { get; set; }
    public int EventoId { get; set; }
    public string EventoName { get; set; } = null!;
    public DateTime EventoDate { get; set; }
    public string EventoLocation { get; set; } = null!;
    public decimal EventoTicketPrice { get; set; }
    public EventStatus EventoStatus { get; set; }
    public decimal? CommissionPorcentaje { get; set; }
    public decimal? CommissionAmount { get; set; }
    public CommissionStatus CommissionStatus { get; set; }
    public InvitationStatus InvitationStatus { get; set; }
}
