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
    public class UnitOfMeasureService : IUnitOfMeasureService
    {
        private readonly AuthorizedHttpClient _httpClient;

        public UnitOfMeasureService(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UnitOfMeasureDto>> GetUnitOfMeasuresAsync()
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
