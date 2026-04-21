using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Domain.Entities;

public class PromotorEvento
{
    public int Id { get; set; }
    public decimal Commission { get; set; }
    public CommissionStatus CommissionStatus { get; set; } = CommissionStatus.Pendiente;
    public InvitationStatus InvitationStatus { get; set; } = InvitationStatus.Pendiente;
    public bool Active { get; set; } = true;

    public int PromotorId { get; set; }
    public Promotor Promotor { get; set; } = null!;
    public int EventoId { get; set; }
    public Evento Evento { get; set; } = null!;
    public ICollection<Invitado> Invitados { get; set; } = new List<Invitado>();
}
