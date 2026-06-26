

using Application.Auth.Interfaces;
using Application.Eventos.Interfaces;
using Application.Invitaciones.Interfaces;
using Domain.Entities;

namespace Application.Invitaciones.Services
{
    public class InvitacionService
    {
        private readonly IInvitacionRepository _invitacionRepository;
        private readonly IEmailService _emailService;
        private readonly IEventoRepository _eventoRepository;

        public InvitacionService(IInvitacionRepository invitacionRepository, IEmailService emailService, IEventoRepository eventoRepository)
        {
            _invitacionRepository = invitacionRepository;
            _emailService = emailService;
            _eventoRepository = eventoRepository;
        }

        public async Task SendInvitacionAsync(string email, int organizadorId, int eventoId, decimal? commission)
        {
            var evento = await _eventoRepository.GetByIdAsync(eventoId, organizadorId);
            if (evento == null)
                throw new Exception("No existe un evento con ese ID.");

            var token = Guid.NewGuid().ToString();

            var invitacion = new Invitacion
            {
                Token = token,
                Email = email,
                OrganizadorId = organizadorId,
                EventoId = eventoId,
                ExpiresAt = DateTime.UtcNow.AddHours(48),
                Used = false,
                Commission = commission
            };

            await _invitacionRepository.CreateAsync(invitacion);
            await _emailService.SendInvitationAsync(email, token, commission, evento.Name, evento.Date, evento.Location, evento.TicketPrice);
        }

        public async Task<Invitacion?> GetByTokenAsync(string token)
        {
            return await _invitacionRepository.GetByTokenAsync(token);
        }
    }
}
