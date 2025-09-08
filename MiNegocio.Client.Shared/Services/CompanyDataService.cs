using MiNegocio.Client.Shared.Interfaces;
using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Client.Shared.Services
{
    public class CompanyDataService : ICompanyDataService
    {
        private readonly AuthorizedHttpClient _httpClient;

        public CompanyDataService(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Métodos para Warehouses
        public async Task<List<WarehouseDto>> GetWarehousesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<WarehouseDto>>("api/warehouse") ?? new List<WarehouseDto>();
        }

        public async Task<WarehouseDto?> GetWarehouseAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<WarehouseDto>($"api/warehouse/{id}");
        }

        public async Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/warehouse", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<WarehouseDto>();
            }
            throw new Exception("Error al crear el almacén");
        }

        public async Task<WarehouseDto?> UpdateWarehouseAsync(UpdateWarehouseRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/warehouse/{request.Id}", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<WarehouseDto>();
            }
            return null;
        }

        public async Task<bool> DeleteWarehouseAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/warehouse/{id}");
            return response.IsSuccessStatusCode;
        }

        // Métodos para Categories
        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryDto>>("api/category") ?? new List<CategoryDto>();
        }

        public async Task<CategoryDto?> GetCategoryAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<CategoryDto>($"api/category/{id}");
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/category", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CategoryDto>();
            }
            throw new Exception("Error al crear la categoría");
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(UpdateCategoryRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/category/{request.Id}", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CategoryDto>();
            }
            return null;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/category/{id}");
            return response.IsSuccessStatusCode;
        }

        // Métodos para UnitsOfMeasure
        public async Task<List<UnitOfMeasureDto>> GetUnitsOfMeasureAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<UnitOfMeasureDto>>("api/unitofmeasure") ?? new List<UnitOfMeasureDto>();
        }

        public async Task<UnitOfMeasureDto?> GetUnitOfMeasureAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<UnitOfMeasureDto>($"api/unitofmeasure/{id}");
        }

        public async Task<UnitOfMeasureDto> CreateUnitOfMeasureAsync(CreateUnitOfMeasureRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/unitofmeasure", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UnitOfMeasureDto>();
            }
            throw new Exception("Error al crear la unidad de medida");
        }

        public async Task<UnitOfMeasureDto?> UpdateUnitOfMeasureAsync(UpdateUnitOfMeasureRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/unitofmeasure/{request.Id}", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UnitOfMeasureDto>();
            }
            return null;
        }

        public async Task<bool> DeleteUnitOfMeasureAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/unitofmeasure/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
