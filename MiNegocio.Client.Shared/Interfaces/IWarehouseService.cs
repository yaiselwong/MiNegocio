using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Client.Shared.Interfaces
{
    public interface IWarehouseService
    {
        Task<List<WarehouseDto>> GetWarehousesByCompanyAsync(int companyId);
        Task<WarehouseDto?> GetWarehouseByIdAsync(int id);
        Task<WarehouseDto?> CreateWarehouseAsync(CreateWarehouseRequest request);
        Task<WarehouseDto?> UpdateWarehouseAsync(int id, CreateWarehouseRequest request);
        Task<bool> DeleteWarehouseAsync(int id);
    }
}
