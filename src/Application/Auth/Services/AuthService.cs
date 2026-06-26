using Application.Auth.DTOs;
using Application.Auth.Interfaces;
using Application.Invitaciones.Interfaces;
using Application.PromotorEventos.Interfaces;
using Application.Users.Dtos;
using Application.Users.Interfaces;

using Domain.Entities;
using Domain.Enums;

namespace Application.Auth.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IInvitacionRepository _invitacionRepository;
    private readonly IPromotorEventoRepository _promotorEventoRepository;

    public AuthService(IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IInvitacionRepository invitacionRepository,
        IPromotorEventoRepository promotorEventoRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _invitacionRepository = invitacionRepository;
        _promotorEventoRepository = promotorEventoRepository;
    }

    public async Task<UsuarioDto> RegisterAsync(RegisterUserDto dto)
    {
        if (await _userRepository.ExistsByEmailAsync(dto.Email))
            throw new Exception("El email ya está en uso.");

        var hashedPassword = _passwordHasher.HashPassword(dto.Password);
        var usuario = Usuario.Create(dto.Name, dto.LastName, dto.Email, hashedPassword, dto.Phone, Role.Organizador);
        var creado = await _userRepository.CreateAsync(usuario);
        return ToDto(creado);
    }

    public async Task<UsuarioDto> RegisterWithInvitacionAsync(RegisterWithInvitacionDto dto)
    {
        var invitacion = await _invitacionRepository.GetByTokenAsync(dto.Token);

        if (invitacion == null || invitacion.Used || invitacion.ExpiresAt < DateTime.UtcNow)
            throw new Exception("El token de invitación es inválido o expiró.");

        // if (invitacion.Email != dto.Email)
        //     throw new Exception("El email no coincide con el de la invitación.");

        if (await _userRepository.ExistsByEmailAsync(dto.Email))
            throw new Exception("El email ya está en uso.");

        var hashedPassword = _passwordHasher.HashPassword(dto.Password);
        var usuario = Usuario.Create(dto.Name, dto.LastName, dto.Email, hashedPassword, dto.Phone, Role.Promotor);
        await _invitacionRepository.MarkAsUsedAsync(invitacion);
        var creado = await _userRepository.CreateAsync(usuario);

        if (invitacion.EventoId > 0)
        {
            await _promotorEventoRepository.CreateAsync(new PromotorEvento
            {
                PromotorId = creado.Id,
                EventoId = invitacion.EventoId,
                Commission = invitacion.Commission,
                InvitationStatus = InvitationStatus.Aceptada
            });
        }

        return ToDto(creado);
    }

    public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        var usuario = await _userRepository.GetByIdAsync(userId);
        if (usuario == null)
            throw new Exception("Usuario no encontrado.");

        if (!_passwordHasher.VerifyPassword(dto.CurrentPassword, usuario.Password))
            throw new Exception("Password actual incorrecta.");

        var newHashedPassword = _passwordHasher.HashPassword(dto.NewPassword);
        await _userRepository.UpdatePasswordAsync(usuario, newHashedPassword);
    }

    public async Task<UsuarioDto?> GetMeAsync(int userId)
    {
        var usuario = await _userRepository.GetByIdAsync(userId);
        return usuario == null ? null : ToDto(usuario);
    }

    private static UsuarioDto ToDto(Usuario u) => new()
    {
        Id = u.Id,
        Name = u.Name,
        LastName = u.Lastname,
        Email = u.Email,
        Phone = u.Phone,
        Active = u.Active
    };

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var usuario = await _userRepository.GetByEmailAsync(dto.Email);
        if (usuario == null)
            throw new Exception("Credenciales inválidas.");

        if (!_passwordHasher.VerifyPassword(dto.Password, usuario.Password))
            throw new Exception("Credenciales inválidas.");

        return _tokenService.GenerateToken(usuario);
    }
}
