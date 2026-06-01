

using Application.Auth.Interfaces;
using Application.Invitaciones.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace Application.Invitaciones.Services
{
    public class InvitacionService
    {
        private readonly IInvitacionRepository _invitacionRepository;
        private readonly IEmailService _emailService;

        public InvitacionService(IInvitacionRepository invitacionRepository, IEmailService emailService)
        {
            _invitacionRepository = invitacionRepository;
            _emailService = emailService;
        }

        public async Task SendInvitacionAsync(string email, Role role, int organizadorId)
        {
            var token = Guid.NewGuid().ToString();

            var invitacion = new Invitacion
            {
                Token = token,
                Email = email,
                Role = role,
                OrganizadorId = organizadorId,
                ExpiresAt = DateTime.UtcNow.AddHours(48),
                Used = false
            };

            await _invitacionRepository.CreateAsync(invitacion);
            await _emailService.SendInvitationAsync(email, token, role);
        }

        public async Task<Invitacion?> GetByTokenAsync(string token)
        {
            return await _invitacionRepository.GetByTokenAsync(token);
        }
    }
}
