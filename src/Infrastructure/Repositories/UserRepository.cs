using Application.Users.Dtos;
using Application.Users.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : Repository<Usuario>, IUserRepository
{
    public UserRepository(ListVIPContext context) : base(context) { }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Usuarios.AnyAsync(u => u.Email == email);
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task UpdatePasswordAsync(Usuario usuario, string newHashedPassword)
    {
        usuario.Password = newHashedPassword;
        await _context.SaveChangesAsync();
    }

    public async Task<UsuarioDto?> ToggleActiveAsync(int promotorId, int organizadorId)
    {
        var usuario = await _context.Usuarios
            .Where(u => u.Id == promotorId && _context.PromotorEventos
                .Any(pe => pe.PromotorId == u.Id &&
                           pe.InvitationStatus == Domain.Enums.InvitationStatus.Aceptada &&
                           _context.Eventos.Any(e => e.Id == pe.EventoId && e.OrganizadorId == organizadorId)))
            .FirstOrDefaultAsync();

        if (usuario == null) return null;

        usuario.Active = !usuario.Active;
        await _context.SaveChangesAsync();

        return new UsuarioDto
        {
            Id = usuario.Id,
            Name = usuario.Name,
            LastName = usuario.Lastname,
            Email = usuario.Email,
            Phone = usuario.Phone,
            Active = usuario.Active
        };
    }

    public async Task<IEnumerable<UsuarioDto>> GetPromotoresByOrganizadorAsync(int organizadorId)
    {
        return await _context.Usuarios
            .Where(u => _context.PromotorEventos
                .Any(pe => pe.PromotorId == u.Id &&
                           pe.InvitationStatus == Domain.Enums.InvitationStatus.Aceptada &&
                           _context.Eventos.Any(e => e.Id == pe.EventoId && e.OrganizadorId == organizadorId)))
            .Select(u => new UsuarioDto
            {
                Id = u.Id,
                Name = u.Name,
                LastName = u.Lastname,
                Email = u.Email,
                Phone = u.Phone,
                Active = u.Active
            })
            .ToListAsync();
    }
}
