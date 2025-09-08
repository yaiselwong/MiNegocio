using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;

namespace MiNegocio.Server.Interfaces
{
    public interface IWarehouseManagementService
    {
        Task<List<WarehouseDto>> GetWarehousesByCompanyAsync(int companyId);
        Task<WarehouseDto?> GetWarehouseByIdAsync(int id);
        Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseRequest request, int companyId);
        Task<WarehouseDto?> UpdateWarehouseAsync(int id, CreateWarehouseRequest request);
        Task<bool> DeleteWarehouseAsync(int id);
    }
}
