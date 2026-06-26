using Application.Common.Interfaces;
using Application.PromotorEventos.Dtos;
using Domain.Entities;

namespace Application.PromotorEventos.Interfaces;

public interface IPromotorEventoRepository : IRepository<PromotorEvento>
{
    Task<IEnumerable<PromotorEventoDto>> GetByEventoIdAsync(int eventoId, int organizadorId);
    Task<bool> ExistsByPromotorAndEventoAsync(int promotorId, int eventoId);
    Task<PromotorEvento?> GetByPromotorAndEventoAsync(int promotorId, int eventoId);
    Task<bool> PromotorPerteneceAlOrganizadorAsync(int promotorId, int organizadorId);
    Task<PromotorEventoDto?> AsignarAsync(int promotorId, int eventoId, decimal commission);
    Task<PromotorEventoDto?> AceptarAsync(int promotorId, int eventoId);
    Task<PromotorEventoDto?> RechazarAsync(int promotorId, int eventoId);
    Task<PromotorEventoDto?> LiquidarAsync(int promotorId, int eventoId, int organizadorId);
    Task CalcularComisionesAsync(int eventoId, decimal ticketPrice);
    Task<IEnumerable<MisEventosDto>> GetEventosByPromotorAsync(int promotorId);
}
