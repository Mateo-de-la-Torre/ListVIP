using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Domain.Entities;

public class Invitado
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public GuestStatus GuestStatus { get; set; } = GuestStatus.EnLista;
    public bool Active { get; set; } = true;

    public int PromotorEventoId { get; set; }
    public PromotorEvento PromotorEvento { get; set; } = null!;
    public RegistroIngreso? RegistroIngreso { get; set; }
}
