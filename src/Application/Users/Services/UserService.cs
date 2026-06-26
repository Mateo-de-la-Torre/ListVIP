using Application.Users.Dtos;
using Application.Users.Interfaces;

namespace Application.Users.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UsuarioDto>> GetPromotoresByOrganizadorAsync(int organizadorId)
    {
        return await _userRepository.GetPromotoresByOrganizadorAsync(organizadorId);
    }

    public async Task<UsuarioDto?> ToggleActiveAsync(int promotorId, int organizadorId)
    {
        return await _userRepository.ToggleActiveAsync(promotorId, organizadorId);
    }
}
