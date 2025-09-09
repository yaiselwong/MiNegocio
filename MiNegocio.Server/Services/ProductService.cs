using Microsoft.EntityFrameworkCore;
using MiNegocio.Server.Interfaces;
using MiNegocio.Shared.Data.UoW;
using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;
using MiNegocio.Shared.Models;

namespace MiNegocio.Server.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ProductDto>> GetProductsByCompanyAsync(int companyId)
        {
            var products = await _unitOfWork.ProductRepository.GetAll()
                .Include(p => p.Category)
                .Include(p => p.UnitOfMeasure)
                .Include(p => p.ProductWarehouses)
                .ThenInclude(pw => pw.Warehouse)
                .Where(p => p.CompanyId == companyId)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Code = p.Code,
                PurchasePrice = p.PurchasePrice,
                SalePrice = p.SalePrice,
                IsActive = p.IsActive,
                CompanyId = p.CompanyId,
                CategoryId = p.CategoryId,
                UnitOfMeasureId = p.UnitOfMeasureId,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                Category = p.Category != null ? new CategoryDto
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name,
                    Description = p.Category.Description,
                    IsActive = p.Category.IsActive,
                    CompanyId = p.Category.CompanyId,
                    CreatedAt = p.Category.CreatedAt,
                    UpdatedAt = p.Category.UpdatedAt,
                    ProductCount = 0
                } : null,
                UnitOfMeasure = p.UnitOfMeasure != null ? new UnitOfMeasureDto
                {
                    Id = p.UnitOfMeasure.Id,
                    Name = p.UnitOfMeasure.Name,
                    Description = p.UnitOfMeasure.Description,
                    Abbreviation = p.UnitOfMeasure.Abbreviation,
                    IsActive = p.UnitOfMeasure.IsActive,
                    CompanyId = p.UnitOfMeasure.CompanyId,
                    CreatedAt = p.UnitOfMeasure.CreatedAt,
                    UpdatedAt = p.UnitOfMeasure.UpdatedAt,
                    ProductCount = 0
                } : null,
                ProductWarehouses = p.ProductWarehouses.Select(pw => new ProductWarehouseDto
                {
                    Id = pw.Id,
                    ProductId = pw.ProductId,
                    WarehouseId = pw.WarehouseId,
                    Quantity = pw.Quantity,
                    MinStock = pw.MinStock,
                    CreatedAt = pw.CreatedAt,
                    UpdatedAt = pw.UpdatedAt,
                    Warehouse = pw.Warehouse != null ? new WarehouseDto
                    {
                        Id = pw.Warehouse.Id,
                        Name = pw.Warehouse.Name,
                        Description = pw.Warehouse.Description,
                        Address = pw.Warehouse.Address,
                        IsActive = pw.Warehouse.IsActive,
                        CompanyId = pw.Warehouse.CompanyId,
                        CreatedAt = pw.Warehouse.CreatedAt,
                        UpdatedAt = pw.Warehouse.UpdatedAt,
                        ProductCount = 0
                    } : null
                }).ToList(),
                TotalQuantity = p.ProductWarehouses.Sum(pw => pw.Quantity)
            }).ToList();
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetAll()
                .Include(p => p.Category)
                .Include(p => p.UnitOfMeasure)
                .Include(p => p.ProductWarehouses)
                .ThenInclude(pw => pw.Warehouse)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Code = product.Code,
                PurchasePrice = product.PurchasePrice,
                SalePrice = product.SalePrice,
                IsActive = product.IsActive,
                CompanyId = product.CompanyId,
                CategoryId = product.CategoryId,
                UnitOfMeasureId = product.UnitOfMeasureId,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                Category = product.Category != null ? new CategoryDto
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name,
                    Description = product.Category.Description,
                    IsActive = product.Category.IsActive,
                    CompanyId = product.Category.CompanyId,
                    CreatedAt = product.Category.CreatedAt,
                    UpdatedAt = product.Category.UpdatedAt,
                    ProductCount = 0
                } : null,
                UnitOfMeasure = product.UnitOfMeasure != null ? new UnitOfMeasureDto
                {
                    Id = product.UnitOfMeasure.Id,
                    Name = product.UnitOfMeasure.Name,
                    Description = product.UnitOfMeasure.Description,
                    Abbreviation = product.UnitOfMeasure.Abbreviation,
                    IsActive = product.UnitOfMeasure.IsActive,
                    CompanyId = product.UnitOfMeasure.CompanyId,
                    CreatedAt = product.UnitOfMeasure.CreatedAt,
                    UpdatedAt = product.UnitOfMeasure.UpdatedAt,
                    ProductCount = 0
                } : null,
                ProductWarehouses = product.ProductWarehouses.Select(pw => new ProductWarehouseDto
                {
                    Id = pw.Id,
                    ProductId = pw.ProductId,
                    WarehouseId = pw.WarehouseId,
                    Quantity = pw.Quantity,
                    MinStock = pw.MinStock,
                    CreatedAt = pw.CreatedAt,
                    UpdatedAt = pw.UpdatedAt,
                    Warehouse = pw.Warehouse != null ? new WarehouseDto
                    {
                        Id = pw.Warehouse.Id,
                        Name = pw.Warehouse.Name,
                        Description = pw.Warehouse.Description,
                        Address = pw.Warehouse.Address,
                        IsActive = pw.Warehouse.IsActive,
                        CompanyId = pw.Warehouse.CompanyId,
                        CreatedAt = pw.Warehouse.CreatedAt,
                        UpdatedAt = pw.Warehouse.UpdatedAt,
                        ProductCount = 0
                    } : null
                }).ToList(),
                TotalQuantity = product.ProductWarehouses.Sum(pw => pw.Quantity)
            };
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductRequest request, int companyId)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Code = request.Code,
                PurchasePrice = request.PurchasePrice,
                SalePrice = request.SalePrice,
                IsActive = request.IsActive,
                CompanyId = companyId,
                CategoryId = request.CategoryId,
                UnitOfMeasureId = request.UnitOfMeasureId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.ProductRepository.Add(product);
            await _unitOfWork.CommitAsync();

            // Add product to warehouses
            foreach (var warehouseRequest in request.Warehouses)
            {
                var productWarehouse = new ProductWarehouse
                {
                    ProductId = product.Id,
                    WarehouseId = warehouseRequest.WarehouseId,
                    Quantity = warehouseRequest.Quantity,
                    MinStock = warehouseRequest.MinStock,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.ProductWarehouseRepository.Add(productWarehouse);
            }

            await _unitOfWork.CommitAsync();

            return await GetProductByIdAsync(product.Id) ?? throw new Exception("Error al crear el producto");
        }

        public async Task<ProductDto?> UpdateProductAsync(UpdateProductRequest request)
        {
            var product = await _unitOfWork.ProductRepository.GetAll()
                .Include(p => p.ProductWarehouses)
                .FirstOrDefaultAsync(p => p.Id == request.Id);

            if (product == null) return null;

            // Update product properties
            product.Name = request.Name;
            product.Description = request.Description;
            product.Code = request.Code;
            product.PurchasePrice = request.PurchasePrice;
            product.SalePrice = request.SalePrice;
            product.IsActive = request.IsActive;
            product.CategoryId = request.CategoryId;
            product.UnitOfMeasureId = request.UnitOfMeasureId;
            product.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.ProductRepository.Update(product);

            // Update product warehouses
            // Remove existing warehouses
            foreach (var existingWarehouse in product.ProductWarehouses.ToList())
            {
                _unitOfWork.ProductWarehouseRepository.Delete(existingWarehouse);
            }

            // Add updated warehouses
            foreach (var warehouseRequest in request.Warehouses)
            {
                var productWarehouse = new ProductWarehouse
                {
                    ProductId = product.Id,
                    WarehouseId = warehouseRequest.WarehouseId,
                    Quantity = warehouseRequest.Quantity,
                    MinStock = warehouseRequest.MinStock,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.ProductWarehouseRepository.Add(productWarehouse);
            }

            await _unitOfWork.CommitAsync();

            return await GetProductByIdAsync(product.Id);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetAll()
                .Include(p => p.ProductWarehouses)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return false;

            // Remove product warehouses first
            foreach (var productWarehouse in product.ProductWarehouses.ToList())
            {
                _unitOfWork.ProductWarehouseRepository.Delete(productWarehouse);
            }

            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<ProductWarehouseDto?> UpdateProductWarehouseAsync(UpdateProductWarehouseRequest request)
        {
            var productWarehouse = await _unitOfWork.ProductWarehouseRepository.FindOneAsync(pw => pw.Id == request.Id);
            if (productWarehouse == null) return null;

            productWarehouse.Quantity = request.Quantity;
            productWarehouse.MinStock = request.MinStock;
            productWarehouse.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.ProductWarehouseRepository.Update(productWarehouse);
            await _unitOfWork.CommitAsync();

            var product = await GetProductByIdAsync(productWarehouse.ProductId);
            return product?.ProductWarehouses.FirstOrDefault(pw => pw.Id == productWarehouse.Id);
        }

        public async Task<List<ProductWarehouseDto>> GetProductWarehousesAsync(int productId)
        {
            var productWarehouses = await _unitOfWork.ProductWarehouseRepository.GetAll()
                .Include(pw => pw.Warehouse)
                .Where(pw => pw.ProductId == productId)
                .OrderBy(pw => pw.Warehouse.Name)
                .ToListAsync();

            return productWarehouses.Select(pw => new ProductWarehouseDto
            {
                Id = pw.Id,
                ProductId = pw.ProductId,
                WarehouseId = pw.WarehouseId,
                Quantity = pw.Quantity,
                MinStock = pw.MinStock,
                CreatedAt = pw.CreatedAt,
                UpdatedAt = pw.UpdatedAt,
                Warehouse = pw.Warehouse != null ? new WarehouseDto
                {
                    Id = pw.Warehouse.Id,
                    Name = pw.Warehouse.Name,
                    Description = pw.Warehouse.Description,
                    Address = pw.Warehouse.Address,
                    IsActive = pw.Warehouse.IsActive,
                    CompanyId = pw.Warehouse.CompanyId,
                    CreatedAt = pw.Warehouse.CreatedAt,
                    UpdatedAt = pw.Warehouse.UpdatedAt,
                    ProductCount = 0
                } : null
            }).ToList();
        }
    public async Task<bool> TransferProductAsync(CreateProductTransferRequest request)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(); // Iniciar transacción

            try
            {
                // Verificar stock disponible en el almacén de origen
                var fromWarehouse = await _unitOfWork.ProductWarehouseRepository.GetAll()
                    .FirstOrDefaultAsync(pw => pw.ProductId == request.ProductId && pw.WarehouseId == request.FromWarehouseId);

                if (fromWarehouse == null || fromWarehouse.Quantity < request.Quantity)
                {
                    throw new Exception("No hay suficiente stock disponible en el almacén de origen");
                }

                // Obtener o crear registro en el almacén de destino
                var toWarehouse = await _unitOfWork.ProductWarehouseRepository.GetAll()
                    .FirstOrDefaultAsync(pw => pw.ProductId == request.ProductId && pw.WarehouseId == request.ToWarehouseId);

                if (toWarehouse == null)
                {
                    toWarehouse = new ProductWarehouse
                    {
                        ProductId = request.ProductId,
                        WarehouseId = request.ToWarehouseId,
                        Quantity = 0,
                        MinStock = 0,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _unitOfWork.ProductWarehouseRepository.Add(toWarehouse);
                }

                // Actualizar cantidades
                fromWarehouse.Quantity -= request.Quantity;
                toWarehouse.Quantity += request.Quantity;

                _unitOfWork.ProductWarehouseRepository.Update(fromWarehouse);
                _unitOfWork.ProductWarehouseRepository.Update(toWarehouse);

                // Crear registro de transferencia
                var transfer = new ProductTransfer
                {
                    ProductId = request.ProductId,
                    FromWarehouseId = request.FromWarehouseId,
                    ToWarehouseId = request.ToWarehouseId,
                    Quantity = request.Quantity,
                    TransferDate = request.TransferDate,
                    Notes = request.Notes,
                    Status = "Completed",
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.ProductTransferRepository.Add(transfer);
                await _unitOfWork.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<ProductTransferDto>> GetProductTransfersAsync(int productId)
        {
            var transfers = await _unitOfWork.ProductTransferRepository.GetAll()
                .Include(t => t.Product)
                .Include(t => t.FromWarehouse)
                .Include(t => t.ToWarehouse)
                .Where(t => t.ProductId == productId)
                .OrderByDescending(t => t.TransferDate)
                .ToListAsync();

            return transfers.Select(t => new ProductTransferDto
            {
                Id = t.Id,
                ProductId = t.ProductId,
                FromWarehouseId = t.FromWarehouseId,
                ToWarehouseId = t.ToWarehouseId,
                Quantity = t.Quantity,
                TransferDate = t.TransferDate,
                Notes = t.Notes,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                Product = t.Product != null ? new ProductDto
                {
                    Id = t.Product.Id,
                    Name = t.Product.Name,
                    Code = t.Product.Code,
                    PurchasePrice = t.Product.PurchasePrice,
                    SalePrice = t.Product.SalePrice,
                    IsActive = t.Product.IsActive,
                    CompanyId = t.Product.CompanyId,
                    CategoryId = t.Product.CategoryId,
                    UnitOfMeasureId = t.Product.UnitOfMeasureId,
                    CreatedAt = t.Product.CreatedAt,
                    UpdatedAt = t.Product.UpdatedAt
                } : null,
                FromWarehouse = t.FromWarehouse != null ? new WarehouseDto
                {
                    Id = t.FromWarehouse.Id,
                    Name = t.FromWarehouse.Name,
                    Description = t.FromWarehouse.Description,
                    Address = t.FromWarehouse.Address,
                    IsActive = t.FromWarehouse.IsActive,
                    CompanyId = t.FromWarehouse.CompanyId,
                    CreatedAt = t.FromWarehouse.CreatedAt,
                    UpdatedAt = t.FromWarehouse.UpdatedAt
                } : null,
                ToWarehouse = t.ToWarehouse != null ? new WarehouseDto
                {
                    Id = t.ToWarehouse.Id,
                    Name = t.ToWarehouse.Name,
                    Description = t.ToWarehouse.Description,
                    Address = t.ToWarehouse.Address,
                    IsActive = t.ToWarehouse.IsActive,
                    CompanyId = t.ToWarehouse.CompanyId,
                    CreatedAt = t.ToWarehouse.CreatedAt,
                    UpdatedAt = t.ToWarehouse.UpdatedAt
                } : null
            }).ToList();
        }
    }
}
