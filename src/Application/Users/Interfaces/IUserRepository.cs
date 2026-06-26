using Application.Common.Interfaces;
using Application.Users.Dtos;
using Domain.Entities;

namespace Application.Users.Interfaces;

public interface IUserRepository : IRepository<Usuario>
{
    Task<bool> ExistsByEmailAsync(string email);
    Task<Usuario?> GetByEmailAsync(string email);
    Task UpdatePasswordAsync(Usuario usuario, string newHashedPassword);
    Task<IEnumerable<UsuarioDto>> GetPromotoresByOrganizadorAsync(int organizadorId);
    Task<UsuarioDto?> ToggleActiveAsync(int promotorId, int organizadorId);
}
