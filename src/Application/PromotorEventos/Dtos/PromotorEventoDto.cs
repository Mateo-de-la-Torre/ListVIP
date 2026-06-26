using Domain.Enums;

namespace Application.PromotorEventos.Dtos;

public class PromotorEventoDto
{
    public int PromotorEventoId { get; set; }
    public int PromotorId { get; set; }
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public decimal? CommissionPorcentaje { get; set; }
    public decimal? CommissionAmount { get; set; }
    public CommissionStatus CommissionStatus { get; set; }
    public InvitationStatus InvitationStatus { get; set; }
}
