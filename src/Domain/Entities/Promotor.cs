using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

public class Promotor : Usuario
{
    public ICollection<PromotorEvento> PromotorEventos { get; set; } = new List<PromotorEvento>();
}