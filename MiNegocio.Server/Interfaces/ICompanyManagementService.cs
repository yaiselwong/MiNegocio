using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Models;

namespace MiNegocio.Server.Interfaces
{
    public interface ICompanyManagementService
    {
        Task<List<Company>> GetAllCompaniesAsync();
        Task<Company?> GetCompanyByIdAsync(int id);
        Task<Company> CreateCompanyAsync(CreateCompanyRequest request);
        Task<Company?> UpdateCompanyAsync(int id, CreateCompanyRequest request);
        Task<bool> DeleteCompanyAsync(int id);
    }
}
