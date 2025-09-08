using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Client.Shared.Interfaces
{
    public interface ICompanyDataService
    {
        // Métodos para Warehouses
        Task<List<WarehouseDto>> GetWarehousesAsync();
        Task<WarehouseDto?> GetWarehouseAsync(int id);
        Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseRequest request);
        Task<WarehouseDto?> UpdateWarehouseAsync(UpdateWarehouseRequest request);
        Task<bool> DeleteWarehouseAsync(int id);

        // Métodos para Categories
        Task<List<CategoryDto>> GetCategoriesAsync();
        Task<CategoryDto?> GetCategoryAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request);
        Task<CategoryDto?> UpdateCategoryAsync(UpdateCategoryRequest request);
        Task<bool> DeleteCategoryAsync(int id);

        // Métodos para UnitsOfMeasure
        Task<List<UnitOfMeasureDto>> GetUnitsOfMeasureAsync();
        Task<UnitOfMeasureDto?> GetUnitOfMeasureAsync(int id);
        Task<UnitOfMeasureDto> CreateUnitOfMeasureAsync(CreateUnitOfMeasureRequest request);
        Task<UnitOfMeasureDto?> UpdateUnitOfMeasureAsync(UpdateUnitOfMeasureRequest request);
        Task<bool> DeleteUnitOfMeasureAsync(int id);
    }
}
