using MiNegocio.Shared.Models;
using System.Security.Claims;

namespace MiNegocio.Server.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        ClaimsPrincipal? ValidateToken(string token);
    }
}
