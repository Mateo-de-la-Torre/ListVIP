using Application.Eventos.Interfaces;
using Application.PromotorEventos.Dtos;
using Application.PromotorEventos.Interfaces;
using Application.Users.Interfaces;
using Domain.Enums;

namespace Application.PromotorEventos.Services;

public class PromotorEventoService
{
    private readonly IPromotorEventoRepository _promotorEventoRepository;
    private readonly IEventoRepository _eventoRepository;
    private readonly IUserRepository _userRepository;

    public PromotorEventoService(IPromotorEventoRepository promotorEventoRepository, IEventoRepository eventoRepository, IUserRepository userRepository)
    {
        _promotorEventoRepository = promotorEventoRepository;
        _eventoRepository = eventoRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<MisEventosDto>> GetEventosByPromotorAsync(int promotorId)
    {
        return await _promotorEventoRepository.GetEventosByPromotorAsync(promotorId);
    }

    public async Task<IEnumerable<PromotorEventoDto>> GetByEventoIdAsync(int eventoId, int organizadorId)
    {
        var evento = await _eventoRepository.GetByIdAsync(eventoId, organizadorId);
        if (evento == null)
            throw new Exception("No existe un evento con ese ID.");

        return await _promotorEventoRepository.GetByEventoIdAsync(eventoId, organizadorId);
    }

    public async Task<PromotorEventoDto> AsignarPromotorAsync(int eventoId, int promotorId, decimal commission, int organizadorId)
    {
        var evento = await _eventoRepository.GetByIdAsync(eventoId, organizadorId);
        if (evento == null)
            throw new Exception("No existe un evento con ese ID.");

        var promotor = await _userRepository.GetByIdAsync(promotorId);
        if (promotor == null)
            throw new Exception("No existe un promotor con ese ID.");

        if (!await _promotorEventoRepository.PromotorPerteneceAlOrganizadorAsync(promotorId, organizadorId))
            throw new Exception("El promotor no pertenece a este organizador.");

        if (await _promotorEventoRepository.ExistsByPromotorAndEventoAsync(promotorId, eventoId))
            throw new Exception("El promotor ya fue invitado a este evento.");

        return (await _promotorEventoRepository.AsignarAsync(promotorId, eventoId, commission))!;
    }

    public async Task<PromotorEventoDto> AceptarAsync(int eventoId, int promotorId)
    {
        var result = await _promotorEventoRepository.AceptarAsync(promotorId, eventoId);
        if (result == null)
            throw new Exception("No se encontró la asignación.");
        return result;
    }

    public async Task<PromotorEventoDto> RechazarAsync(int eventoId, int promotorId)
    {
        var result = await _promotorEventoRepository.RechazarAsync(promotorId, eventoId);
        if (result == null)
            throw new Exception("No se encontró la asignación.");
        return result;
    }

    public async Task<PromotorEventoDto> LiquidarAsync(int eventoId, int promotorId, int organizadorId)
    {
        var evento = await _eventoRepository.GetByIdAsync(eventoId, organizadorId);
        if (evento == null)
            throw new Exception("No existe un evento con ese ID.");

        if (evento.EventStatus != EventStatus.Finalizado)
            throw new Exception("Solo se pueden liquidar comisiones de eventos finalizados.");

        var result = await _promotorEventoRepository.LiquidarAsync(promotorId, eventoId, organizadorId);
        if (result == null)
            throw new Exception("No se encontró la asignación del promotor en este evento.");
        return result;
    }
}
