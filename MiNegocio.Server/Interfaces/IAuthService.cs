using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;
using MiNegocio.Shared.Models;

namespace MiNegocio.Server.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<AuthResponse> RegisterAsync(User user, string password);
    }
}
