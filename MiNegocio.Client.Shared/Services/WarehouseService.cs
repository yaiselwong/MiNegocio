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
    public class WarehouseService : IWarehouseService
    {
        private readonly AuthorizedHttpClient _httpClient;

        public WarehouseService(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

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
    }
}
