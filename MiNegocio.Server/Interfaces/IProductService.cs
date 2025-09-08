using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;

namespace MiNegocio.Server.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProductsByCompanyAsync(int companyId);
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductRequest request, int companyId);
        Task<ProductDto?> UpdateProductAsync(UpdateProductRequest request);
        Task<bool> DeleteProductAsync(int id);
        Task<ProductWarehouseDto?> UpdateProductWarehouseAsync(UpdateProductWarehouseRequest request);
        Task<List<ProductWarehouseDto>> GetProductWarehousesAsync(int productId);
    }
}
