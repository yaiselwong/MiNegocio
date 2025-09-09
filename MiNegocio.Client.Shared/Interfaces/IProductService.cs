using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Client.Shared.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProductsAsync();
        Task<ProductDto?> GetProductAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductRequest request);
        Task<ProductDto?> UpdateProductAsync(UpdateProductRequest request);
        Task<bool> DeleteProductAsync(int id);
        Task<ProductWarehouseDto?> UpdateProductWarehouseAsync(UpdateProductWarehouseRequest request);
        Task<List<ProductWarehouseDto>> GetProductWarehousesAsync(int productId);
        Task<bool> TransferProductAsync(CreateProductTransferRequest request);
        Task<List<ProductTransferDto>> GetProductTransfersAsync(int productId);
    }
}
