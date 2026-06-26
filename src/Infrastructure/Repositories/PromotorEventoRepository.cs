using Application.PromotorEventos.Dtos;
using Application.PromotorEventos.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PromotorEventoRepository : Repository<PromotorEvento>, IPromotorEventoRepository
{
    public PromotorEventoRepository(ListVIPContext context) : base(context) { }

    public async Task<bool> ExistsByPromotorAndEventoAsync(int promotorId, int eventoId)
    {
        return await _context.PromotorEventos
            .AnyAsync(pe => pe.PromotorId == promotorId && pe.EventoId == eventoId);
    }

    public async Task<PromotorEvento?> GetByPromotorAndEventoAsync(int promotorId, int eventoId)
    {
        return await _context.PromotorEventos
            .FirstOrDefaultAsync(pe => pe.PromotorId == promotorId && pe.EventoId == eventoId);
    }

    public async Task<bool> PromotorPerteneceAlOrganizadorAsync(int promotorId, int organizadorId)
    {
        return await _context.PromotorEventos
            .AnyAsync(pe => pe.PromotorId == promotorId &&
                            pe.InvitationStatus == InvitationStatus.Aceptada &&
                            _context.Eventos.Any(e => e.Id == pe.EventoId && e.OrganizadorId == organizadorId));
    }

    public async Task<IEnumerable<PromotorEventoDto>> GetByEventoIdAsync(int eventoId, int organizadorId)
    {
        return await _context.PromotorEventos
            .Where(pe => pe.EventoId == eventoId &&
                         _context.Eventos.Any(e => e.Id == eventoId && e.OrganizadorId == organizadorId))
            .Select(pe => new PromotorEventoDto
            {
                PromotorEventoId = pe.Id,
                PromotorId = pe.PromotorId,
                Name = pe.Promotor.Name,
                LastName = pe.Promotor.Lastname,
                Email = pe.Promotor.Email,
                CommissionPorcentaje = pe.Commission,
                CommissionAmount = pe.CommissionAmount,
                CommissionStatus = pe.CommissionStatus,
                InvitationStatus = pe.InvitationStatus
            })
            .ToListAsync();
    }

    public async Task<PromotorEventoDto?> AsignarAsync(int promotorId, int eventoId, decimal commission)
    {
        var promotorEvento = new PromotorEvento
        {
            PromotorId = promotorId,
            EventoId = eventoId,
            Commission = commission,
            InvitationStatus = InvitationStatus.Pendiente
        };

        _context.PromotorEventos.Add(promotorEvento);
        await _context.SaveChangesAsync();
        await _context.Entry(promotorEvento).Reference(pe => pe.Promotor).LoadAsync();

        return ToDto(promotorEvento);
    }

    public async Task<PromotorEventoDto?> AceptarAsync(int promotorId, int eventoId)
    {
        var pe = await _context.PromotorEventos
            .Include(x => x.Promotor)
            .FirstOrDefaultAsync(x => x.PromotorId == promotorId && x.EventoId == eventoId);

        if (pe == null) return null;

        pe.InvitationStatus = InvitationStatus.Aceptada;
        await _context.SaveChangesAsync();
        return ToDto(pe);
    }

    public async Task<PromotorEventoDto?> RechazarAsync(int promotorId, int eventoId)
    {
        var pe = await _context.PromotorEventos
            .Include(x => x.Promotor)
            .FirstOrDefaultAsync(x => x.PromotorId == promotorId && x.EventoId == eventoId);

        if (pe == null) return null;

        pe.InvitationStatus = InvitationStatus.Rechazada;
        await _context.SaveChangesAsync();
        return ToDto(pe);
    }

    public async Task<PromotorEventoDto?> LiquidarAsync(int promotorId, int eventoId, int organizadorId)
    {
        var pe = await _context.PromotorEventos
            .Include(x => x.Promotor)
            .FirstOrDefaultAsync(x => x.PromotorId == promotorId &&
                                      x.EventoId == eventoId &&
                                      _context.Eventos.Any(e => e.Id == eventoId && e.OrganizadorId == organizadorId));

        if (pe == null) return null;

        pe.CommissionStatus = CommissionStatus.Liquidada;
        await _context.SaveChangesAsync();
        return ToDto(pe);
    }

    public async Task<IEnumerable<MisEventosDto>> GetEventosByPromotorAsync(int promotorId)
    {
        return await _context.PromotorEventos
            .Where(pe => pe.PromotorId == promotorId &&
                         pe.InvitationStatus != InvitationStatus.Rechazada &&
                         pe.Evento.Active)
            .Select(pe => new MisEventosDto
            {
                PromotorEventoId = pe.Id,
                EventoId = pe.EventoId,
                EventoName = pe.Evento.Name,
                EventoDate = pe.Evento.Date,
                EventoLocation = pe.Evento.Location,
                EventoTicketPrice = pe.Evento.TicketPrice,
                EventoStatus = pe.Evento.EventStatus,
                CommissionPorcentaje = pe.Commission,
                CommissionAmount = pe.CommissionAmount,
                CommissionStatus = pe.CommissionStatus,
                InvitationStatus = pe.InvitationStatus
            })
            .ToListAsync();
    }

    public async Task CalcularComisionesAsync(int eventoId, decimal ticketPrice)
    {
        var promotorEventos = await _context.PromotorEventos
            .Where(pe => pe.EventoId == eventoId && pe.InvitationStatus == InvitationStatus.Aceptada)
            .ToListAsync();

        foreach (var pe in promotorEventos)
        {
            if (pe.Commission.HasValue)
            {
                var ingresados = await _context.Invitados
                    .CountAsync(i => i.PromotorEventoId == pe.Id && i.GuestStatus == GuestStatus.Ingreso);

                pe.CommissionAmount = pe.Commission.Value / 100 * ticketPrice * ingresados;
            }
        }

        await _context.SaveChangesAsync();
    }

    private static PromotorEventoDto ToDto(PromotorEvento pe) => new()
    {
        PromotorEventoId = pe.Id,
        PromotorId = pe.PromotorId,
        Name = pe.Promotor.Name,
        LastName = pe.Promotor.Lastname,
        Email = pe.Promotor.Email,
        CommissionPorcentaje = pe.Commission,
        CommissionAmount = pe.CommissionAmount,
        CommissionStatus = pe.CommissionStatus,
        InvitationStatus = pe.InvitationStatus
    };
}
