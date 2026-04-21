using System;
using System.Collections.Generic;
using System.Text;
using Domain.Enums;

namespace Domain.Entities;

public class Evento
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Venue { get; set; } = null!;
    public int Capacity { get; set; }
    public decimal TicketPrice { get; set; }
    public EventStatus EventStatus { get; set; } = EventStatus.Borrador;
    public bool Active { get; set; } = true;

    public int OrganizadorId { get; set; }
    public Organizador Organizador { get; set; } = null!;
    public ICollection<PromotorEvento> PromotorEventos { get; set; } = new List<PromotorEvento>();
    public ICollection<RegistroIngreso> RegistrosIngreso { get; set; } = new List<RegistroIngreso>();
}
