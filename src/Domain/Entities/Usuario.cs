using Domain.Enums; 
using System;
using System.Collections.Generic;
using System.Text;


namespace Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public Role Role { get; set; }
    public bool Active { get; set; } = true;
}
