using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

public class Portero : Usuario
{
    public ICollection<RegistroIngreso> RegistrosIngreso { get; set; } = new List<RegistroIngreso>();
}