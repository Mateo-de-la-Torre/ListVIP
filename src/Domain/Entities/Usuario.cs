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



    public static Usuario Create(string name, string lastName, string email, string password, string phone, Role role)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre es requerido.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("El apellido es requerido.");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El correo electrónico es requerido.");

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("La contraseña es requerida.");


        return role switch
        {
            Role.Organizador => new Organizador { Name = name, Lastname = lastName, Email = email, Password = password, Phone = phone, Role = role, Active = true },
            Role.Promotor => new Promotor { Name = name, Lastname = lastName, Email = email, Password = password, Phone = phone, Role = role, Active = true },
            Role.Portero => new Portero { Name = name, Lastname = lastName, Email = email, Password = password, Phone = phone, Role = role, Active = true },
            _ => throw new ArgumentException("Rol inválido.")
        };
    }
}