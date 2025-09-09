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
    public class ProductService : IProductService
    {
        private readonly AuthorizedHttpClient _httpClient;

        public ProductService(AuthorizedHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ProductDto>>("api/product") ?? new List<ProductDto>();
        }

        public async Task<ProductDto?> GetProductAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ProductDto>($"api/product/{id}");
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/product", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductDto>();
            }
            throw new Exception("Error al crear el producto");
        }

        public async Task<ProductDto?> UpdateProductAsync(UpdateProductRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/product/{request.Id}", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductDto>();
            }
            return null;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/product/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<ProductWarehouseDto?> UpdateProductWarehouseAsync(UpdateProductWarehouseRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/product/warehouse/{request.Id}", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductWarehouseDto>();
            }
            return null;
        }

        public async Task<List<ProductWarehouseDto>> GetProductWarehousesAsync(int productId)
        {
            return await _httpClient.GetFromJsonAsync<List<ProductWarehouseDto>>($"api/product/{productId}/warehouses") ?? new List<ProductWarehouseDto>();
        }
        public async Task<bool> TransferProductAsync(CreateProductTransferRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/product/transfer", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ProductTransferDto>> GetProductTransfersAsync(int productId)
        {
            return await _httpClient.GetFromJsonAsync<List<ProductTransferDto>>($"api/product/{productId}/transfers") ?? new List<ProductTransferDto>();
        }
    }
}
