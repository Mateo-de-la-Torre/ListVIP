

using Domain.Entities;

namespace Application.Auth.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Usuario usuario);
    }
}
