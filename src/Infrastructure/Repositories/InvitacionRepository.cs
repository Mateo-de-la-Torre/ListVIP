using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Application.Invitaciones.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class InvitacionRepository : IInvitacionRepository
    {
        private readonly ListVIPContext _context;

        public InvitacionRepository(ListVIPContext context)
        {
            _context = context;
        }

        public Task<Invitacion?> GetByTokenAsync(string token)
        {
            return _context.Invitaciones.FirstOrDefaultAsync(i => i.Token == token);
        }

        public Task CreateAsync(Invitacion invitacion)
        {
            _context.Invitaciones.Add(invitacion);
            return _context.SaveChangesAsync();
        }

        public async Task MarkAsUsedAsync(Invitacion invitacion)
        {
            invitacion.Used = true;
            await _context.SaveChangesAsync();
        }
    }
}
