using Application.Auth.DTOs;
using Application.Auth.Interfaces;
using Application.Invitaciones.Interfaces;
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


    public AuthService(IUserRepository userRepository
        , IPasswordHasher passwordHasher
        , ITokenService tokenService
        , IInvitacionRepository invitacionRepository
        )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _invitacionRepository = invitacionRepository;
    }

    public async Task<Usuario> RegisterAsync(RegisterUserDto dto)
    {
        if (await _userRepository.ExistsByEmailAsync(dto.Email))
            throw new Exception("El email ya está en uso.");

        var hashedPassword = _passwordHasher.HashPassword(dto.Password);
        var usuario = Usuario.Create(dto.Name, dto.LastName, dto.Email, hashedPassword, dto.Phone, Role.Organizador);
        return await _userRepository.CreateAsync(usuario);
    }

    public async Task<Usuario> RegisterWithInvitacionAsync(RegisterWithInvitacionDto dto)
    {
        var invitacion = await _invitacionRepository.GetByTokenAsync(dto.Token);

        if (invitacion == null || invitacion.Used || invitacion.ExpiresAt < DateTime.UtcNow)
            throw new Exception("El token de invitación es inválido o expiró.");

        if (invitacion.Email != dto.Email)
            throw new Exception("El email no coincide con el de la invitación.");

        if (await _userRepository.ExistsByEmailAsync(dto.Email))
            throw new Exception("El email ya está en uso.");

        var hashedPassword = _passwordHasher.HashPassword(dto.Password);
        var usuario = Usuario.Create(dto.Name, dto.LastName, dto.Email, hashedPassword, dto.Phone, invitacion.Role);
        await _invitacionRepository.MarkAsUsedAsync(invitacion);
        return await _userRepository.CreateAsync(usuario);
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

    public async Task<Usuario?> GetMeAsync(int userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

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
