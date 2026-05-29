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
    public string Location { get; set; } = null!;
    public int Capacity { get; set; }
    public decimal TicketPrice { get; set; }
    public EventStatus EventStatus { get; set; } = EventStatus.Borrador;
    public bool Active { get; set; } = true;

    public int OrganizadorId { get; set; }
    public Organizador Organizador { get; set; } = null!;
    public ICollection<PromotorEvento> PromotorEventos { get; set; } = new List<PromotorEvento>();
    public ICollection<RegistroIngreso> RegistrosIngreso { get; set; } = new List<RegistroIngreso>();

    public static Evento Create(string name, DateTime date, string location, int capacity, decimal ticketPrice, int organizadorId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del evento es requerido.");

        if (date <= DateTime.UtcNow)
            throw new ArgumentException("La fecha del evento debe ser futura.");

        if (string.IsNullOrWhiteSpace(location))
            throw new ArgumentException("El lugar del evento es requerido.");

        if (capacity <= 0)
            throw new ArgumentException("La capacidad debe ser mayor a 0.");

        if (ticketPrice < 0)
            throw new ArgumentException("El precio no puede ser negativo.");

        return new Evento
        {
            Name = name,
            Date = date,
            Location = location,
            Capacity = capacity,
            TicketPrice = ticketPrice,
            OrganizadorId = organizadorId,
            EventStatus = EventStatus.Borrador,
            Active = true
        };
    }

    public static Evento Update(Evento evento, string? name, DateTime? date, string? location, int? capacity, decimal? ticketPrice)
    {
        if (name != null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("El nombre del evento es requerido.");
            evento.Name = name;
        }
        if (date != null)
        {
            if (date <= DateTime.UtcNow)
                throw new ArgumentException("La fecha del evento debe ser futura.");
            evento.Date = date.Value;
        }
        if (location != null)
        {
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("El lugar del evento es requerido.");
            evento.Location = location;
        }
        if (capacity != null)
        {
            if (capacity <= 0)
                throw new ArgumentException("La capacidad debe ser mayor a 0.");
            evento.Capacity = capacity.Value;
        }
        if (ticketPrice != null)
        {
            if (ticketPrice < 0)
                throw new ArgumentException("El precio no puede ser negativo.");
            evento.TicketPrice = ticketPrice.Value;
        }
        return evento;
    }
}
