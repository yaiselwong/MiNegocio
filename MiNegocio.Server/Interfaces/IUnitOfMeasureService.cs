using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;

namespace MiNegocio.Server.Interfaces
{
    public interface IUnitOfMeasureService
    {
        Task<List<UnitOfMeasureDto>> GetUnitOfMeasuresByCompanyAsync(int companyId);
        Task<UnitOfMeasureDto?> GetUnitOfMeasureByIdAsync(int id);
        Task<UnitOfMeasureDto> CreateUnitOfMeasureAsync(CreateUnitOfMeasureRequest request, int companyId);
        Task<UnitOfMeasureDto?> UpdateUnitOfMeasureAsync(UpdateUnitOfMeasureRequest request);
        Task<bool> DeleteUnitOfMeasureAsync(int id);
    }
}
