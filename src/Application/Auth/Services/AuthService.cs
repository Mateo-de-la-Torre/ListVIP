using Application.Auth.DTOs;
using Application.Auth.Interfaces;
using Application.Users.Interfaces;

using Domain.Entities;
using Domain.Enums;

namespace Application.Auth.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;


    public AuthService(IUserRepository userRepository
        , IPasswordHasher passwordHasher
        , ITokenService tokenService
        )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<Usuario> RegisterAsync(RegisterUserDto dto)
    {
        if (await _userRepository.ExistsByEmailAsync(dto.Email))
            throw new Exception("El email ya está en uso.");

        var hashedPassword = _passwordHasher.HashPassword(dto.Password);
        var usuario = Usuario.Create(dto.Name, dto.LastName, dto.Email, hashedPassword, dto.Phone, Role.Organizador);
        return await _userRepository.CreateAsync(usuario);
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
