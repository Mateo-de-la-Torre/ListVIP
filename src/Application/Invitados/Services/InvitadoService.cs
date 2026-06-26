using Application.Auth.Interfaces;
using Application.Common.Interfaces;
using Application.Eventos.Interfaces;
using Application.Invitados.Dtos;
using Application.Invitados.Interfaces;
using Application.PromotorEventos.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Invitados.Services;

public class InvitadoService
{
    private readonly IInvitadoRepository _invitadoRepository;
    private readonly IPromotorEventoRepository _promotorEventoRepository;
    private readonly IEventoRepository _eventoRepository;
    private readonly IQrService _qrService;
    private readonly IEmailService _emailService;

    public InvitadoService(
        IInvitadoRepository invitadoRepository,
        IPromotorEventoRepository promotorEventoRepository,
        IEventoRepository eventoRepository,
        IQrService qrService,
        IEmailService emailService)
    {
        _invitadoRepository = invitadoRepository;
        _promotorEventoRepository = promotorEventoRepository;
        _eventoRepository = eventoRepository;
        _qrService = qrService;
        _emailService = emailService;
    }

    public async Task<CheckInDto> CheckInAsync(int invitadoId, int promotorId)
    {
        var result = await _invitadoRepository.CheckInAsync(invitadoId, promotorId);
        if (result == null)
            throw new Exception("No se encontró el invitado.");
        return result;
    }

    public async Task DeleteGuestAsync(int invitadoId, int promotorId)
    {
        var deleted = await _invitadoRepository.DeleteAsync(invitadoId, promotorId);
        if (!deleted)
            throw new Exception("No se encontró el invitado.");
    }

    public async Task<IEnumerable<InvitadoDto>> GetGuestsAsync(int eventoId, int promotorId)
    {
        var promotorEvento = await _promotorEventoRepository.GetByPromotorAndEventoAsync(promotorId, eventoId);
        if (promotorEvento == null)
            throw new Exception("No se encontró la asignación del promotor a este evento.");

        return await _invitadoRepository.GetByPromotorEventoAsync(promotorEvento.Id);
    }

    public async Task<InvitadoDto> AddGuestAsync(int eventoId, int promotorId, CreateInvitadoDto dto)
    {
        var promotorEvento = await _promotorEventoRepository.GetByPromotorAndEventoAsync(promotorId, eventoId);
        if (promotorEvento == null)
            throw new Exception("No se encontró la asignación del promotor a este evento.");

        if (promotorEvento.InvitationStatus != InvitationStatus.Aceptada)
            throw new Exception("El promotor no ha aceptado la asignación a este evento.");

        var evento = await _eventoRepository.GetByIdAsync(eventoId);
        if (evento == null)
            throw new Exception("No existe un evento con ese ID.");

        var invitado = new Invitado
        {
            Name = dto.Name,
            Lastname = dto.LastName,
            Phone = dto.Phone,
            Email = dto.Email,
            PromotorEventoId = promotorEvento.Id
        };

        var created = await _invitadoRepository.CreateInvitadoAsync(invitado);

        var qrBase64 = _qrService.GenerateBase64($"LISTVIP-{created!.Id}");
        await _emailService.SendGuestQrAsync(dto.Email, dto.Name, qrBase64, evento.Name, evento.Date, evento.Location);

        return created;
    }
}
