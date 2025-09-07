using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Client.Shared.Interfaces
{
    public interface IAdminService
    {
        // User Management
        Task<List<User>> GetAllUsersAsync();
        Task<List<User>> GetUsersByCompanyAsync(int companyId);
        Task<User?> GetUserAsync(int id);
        Task<User?> CreateUserAsync(CreateUserRequest request);
        Task<User?> UpdateUserAsync(UpdateUserRequest request);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> ResetPasswordAsync(int id, string newPassword);

        // Company Management
        Task<List<Company>> GetAllCompaniesAsync();
        Task<Company?> GetCompanyAsync(int id);
        Task<Company?> CreateCompanyAsync(CreateCompanyRequest request);
        Task<Company?> UpdateCompanyAsync(int id, CreateCompanyRequest request);
        Task<bool> DeleteCompanyAsync(int id);
    }
}
