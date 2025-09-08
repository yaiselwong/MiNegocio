using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Client.Shared.Interfaces
{
    public interface IUnitOfMeasureService
    {
        Task<List<UnitOfMeasureDto>> GetUnitOfMeasuresAsync();
        Task<UnitOfMeasureDto?> GetUnitOfMeasureAsync(int id);
        Task<UnitOfMeasureDto> CreateUnitOfMeasureAsync(CreateUnitOfMeasureRequest request);
        Task<UnitOfMeasureDto?> UpdateUnitOfMeasureAsync(UpdateUnitOfMeasureRequest request);
        Task<bool> DeleteUnitOfMeasureAsync(int id);
    }
}
