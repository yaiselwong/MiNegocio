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
    public class CategoryService : ICategoryService
    {
        private readonly AuthorizedHttpClient _httpClient;

        public CategoryService(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

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
    }
}
