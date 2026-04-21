using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

public class RegistroIngreso
{
    public int Id { get; set; }
    public DateTime AccessTime { get; set; } = DateTime.UtcNow;

    public int InvitadoId { get; set; }
    public Invitado Invitado { get; set; } = null!;
    public int PorteroId { get; set; }
    public Portero Portero { get; set; } = null!;
    public int EventoId { get; set; }
    public Evento Evento { get; set; } = null!;
}
