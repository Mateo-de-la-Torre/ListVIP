using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

public class Organizador : Usuario
{
    public ICollection<Evento> Eventos { get; set; } = new List<Evento>();
}
