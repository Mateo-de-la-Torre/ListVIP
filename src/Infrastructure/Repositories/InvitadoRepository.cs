using Application.Invitados.Dtos;
using Application.Invitados.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class InvitadoRepository : Repository<Invitado>, IInvitadoRepository
{
    public InvitadoRepository(ListVIPContext context) : base(context) { }

    public async Task<InvitadoDto?> CreateInvitadoAsync(Invitado invitado)
    {
        _context.Invitados.Add(invitado);
        await _context.SaveChangesAsync();

        return new InvitadoDto
        {
            Id = invitado.Id,
            Name = invitado.Name,
            LastName = invitado.Lastname,
            Phone = invitado.Phone,
            Email = invitado.Email,
            GuestStatus = invitado.GuestStatus
        };
    }

    public async Task<IEnumerable<InvitadoDto>> GetByPromotorEventoAsync(int promotorEventoId)
    {
        return await _context.Invitados
            .Where(i => i.PromotorEventoId == promotorEventoId && i.Active)
            .Select(i => new InvitadoDto
            {
                Id = i.Id,
                Name = i.Name,
                LastName = i.Lastname,
                Phone = i.Phone,
                Email = i.Email,
                GuestStatus = i.GuestStatus
            })
            .ToListAsync();
    }

    public async Task<CheckInDto?> CheckInAsync(int invitadoId, int promotorId)
    {
        var invitado = await _context.Invitados
            .Include(i => i.PromotorEvento)
                .ThenInclude(pe => pe.Evento)
            .FirstOrDefaultAsync(i => i.Id == invitadoId &&
                _context.PromotorEventos.Any(pe => pe.Id == i.PromotorEventoId && pe.PromotorId == promotorId));

        if (invitado == null) return null;

        if (invitado.PromotorEvento.Evento.EventStatus != EventStatus.EnCurso)
            throw new Exception("El evento no está en curso.");

        if (invitado.GuestStatus != GuestStatus.EnLista)
            throw new Exception("El invitado ya registró su ingreso.");

        invitado.GuestStatus = GuestStatus.Ingreso;

        var registro = new RegistroIngreso
        {
            InvitadoId = invitado.Id,
            EventoId = invitado.PromotorEvento.EventoId,
            AccessTime = DateTime.UtcNow
        };

        _context.RegistrosIngreso.Add(registro);
        await _context.SaveChangesAsync();

        return new CheckInDto
        {
            InvitadoId = invitado.Id,
            Name = invitado.Name,
            LastName = invitado.Lastname,
            GuestStatus = invitado.GuestStatus,
            AccessTime = registro.AccessTime
        };
    }

    public async Task MarcarNoSePresentaronAsync(int eventoId)
    {
        var invitados = await _context.Invitados
            .Where(i => i.GuestStatus == GuestStatus.EnLista &&
                        _context.PromotorEventos.Any(pe => pe.Id == i.PromotorEventoId && pe.EventoId == eventoId))
            .ToListAsync();

        foreach (var invitado in invitados)
            invitado.GuestStatus = GuestStatus.NoSePResento;

        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int invitadoId, int promotorId)
    {
        var invitado = await _context.Invitados
            .FirstOrDefaultAsync(i => i.Id == invitadoId &&
                _context.PromotorEventos.Any(pe => pe.Id == i.PromotorEventoId && pe.PromotorId == promotorId));

        if (invitado == null) return false;

        invitado.Active = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
