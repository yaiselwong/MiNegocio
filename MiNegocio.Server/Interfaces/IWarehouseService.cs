using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;

namespace MiNegocio.Server.Interfaces
{
    public interface IWarehouseService
    {
        Task<List<WarehouseDto>> GetWarehousesByCompanyAsync(int companyId);
        Task<WarehouseDto?> GetWarehouseByIdAsync(int id);
        Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseRequest request, int companyId);
        Task<WarehouseDto?> UpdateWarehouseAsync(UpdateWarehouseRequest request);
        Task<bool> DeleteWarehouseAsync(int id);
    }
}
