using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Models;

namespace MiNegocio.Server.Interfaces
{
    public interface IUserManagementService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<List<User>> GetUsersByCompanyAsync(int companyId);
        Task<User> CreateUserAsync(CreateUserRequest request);
        Task<User?> UpdateUserAsync(UpdateUserRequest request);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> ResetPasswordAsync(int id, string newPassword);
    }
}
