using Microsoft.AspNetCore.Components.Authorization;
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
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;

        public WarehouseService(
            HttpClient httpClient,
            AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
        }

        public async Task<List<WarehouseDto>> GetWarehousesByCompanyAsync(int companyId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<WarehouseDto>>($"api/WarehouseManagement/company/{companyId}");
            return response ?? new List<WarehouseDto>();
        }

        public async Task<WarehouseDto?> GetWarehouseByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<WarehouseDto>($"api/WarehouseManagement/{id}");
        }

        public async Task<WarehouseDto?> CreateWarehouseAsync(CreateWarehouseRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/WarehouseManagement", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<WarehouseDto>();
            }

            return null;
        }

        public async Task<WarehouseDto?> UpdateWarehouseAsync(int id, CreateWarehouseRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/WarehouseManagement/{id}", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<WarehouseDto>();
            }

            return null;
        }

        public async Task<bool> DeleteWarehouseAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/WarehouseManagement/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
