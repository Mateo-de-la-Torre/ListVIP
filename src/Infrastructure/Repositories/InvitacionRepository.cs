using Application.Invitaciones.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class InvitacionRepository : Repository<Invitacion>, IInvitacionRepository
{
    public InvitacionRepository(ListVIPContext context) : base(context) { }

    public Task<Invitacion?> GetByTokenAsync(string token)
    {
        return _context.Invitaciones.FirstOrDefaultAsync(i => i.Token == token);
    }

    public async Task MarkAsUsedAsync(Invitacion invitacion)
    {
        invitacion.Used = true;
        await _context.SaveChangesAsync();
    }
}
