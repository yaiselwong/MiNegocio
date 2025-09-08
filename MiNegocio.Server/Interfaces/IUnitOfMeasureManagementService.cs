using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;

namespace MiNegocio.Server.Interfaces
{
    public interface IUnitOfMeasureManagementService
    {
        Task<List<UnitOfMeasureDto>> GetUnitsOfMeasureByCompanyAsync(int companyId);
        Task<UnitOfMeasureDto?> GetUnitOfMeasureByIdAsync(int id);
        Task<UnitOfMeasureDto> CreateUnitOfMeasureAsync(CreateUnitOfMeasureRequest request, int companyId);
        Task<UnitOfMeasureDto?> UpdateUnitOfMeasureAsync(int id, CreateUnitOfMeasureRequest request);
        Task<bool> DeleteUnitOfMeasureAsync(int id);
    }
}
