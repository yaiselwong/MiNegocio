using Microsoft.EntityFrameworkCore;
using MiNegocio.Server.Interfaces;
using MiNegocio.Shared.Data.UoW;
using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;
using MiNegocio.Shared.Models;

namespace MiNegocio.Server.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WarehouseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<WarehouseDto>> GetWarehousesByCompanyAsync(int companyId)
        {
            var warehouses = await _unitOfWork.WarehouseRepository.GetAll()
                .Include(w => w.Products)
                .Where(w => w.CompanyId == companyId)
                .OrderBy(w => w.Name)
                .ToListAsync();

            return warehouses.Select(w => new WarehouseDto
            {
                Id = w.Id,
                Name = w.Name,
                Description = w.Description,
                Address= w.Address,
                IsActive = w.IsActive,
                CompanyId = w.CompanyId,
                CreatedAt = w.CreatedAt,
                UpdatedAt = w.UpdatedAt,
                ProductCount = w.Products.Count
            }).ToList();
        }

        public async Task<WarehouseDto?> GetWarehouseByIdAsync(int id)
        {
            var warehouse = await _unitOfWork.WarehouseRepository.GetAll()
                .Include(w => w.Products)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse == null) return null;

            return new WarehouseDto
            {
                Id = warehouse.Id,
                Name = warehouse.Name,
                Description = warehouse.Description,
                Address = warehouse.Address,
                IsActive = warehouse.IsActive,
                CompanyId = warehouse.CompanyId,
                CreatedAt = warehouse.CreatedAt,
                UpdatedAt = warehouse.UpdatedAt,
                ProductCount = warehouse.Products.Count
            };
        }

        public async Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseRequest request, int companyId)
        {
            var warehouse = new Warehouse
            {
                Name = request.Name,
                Description = request.Description,
                Address = request.Address,
                IsActive = request.IsActive,
                CompanyId = companyId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.WarehouseRepository.Add(warehouse);
            await _unitOfWork.CommitAsync();

            return await GetWarehouseByIdAsync(warehouse.Id) ?? throw new Exception("Error al crear el almacén");
        }

        public async Task<WarehouseDto?> UpdateWarehouseAsync(UpdateWarehouseRequest request)
        {
            var warehouse = await _unitOfWork.WarehouseRepository.FindOneAsync(w => w.Id == request.Id);
            if (warehouse == null) return null;

            warehouse.Name = request.Name;
            warehouse.Description = request.Description;
            warehouse.Address = request.Address;
            warehouse.IsActive = request.IsActive;
            warehouse.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.WarehouseRepository.Update(warehouse);
            await _unitOfWork.CommitAsync();

            return await GetWarehouseByIdAsync(warehouse.Id);
        }

        public async Task<bool> DeleteWarehouseAsync(int id)
        {
            var warehouse = await _unitOfWork.WarehouseRepository.GetAll()
                .Include(w => w.Products)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (warehouse == null) return false;

            // Verificar si hay productos asociados
            if (warehouse.Products.Any())
            {
                return false;
            }

             _unitOfWork.WarehouseRepository.Delete(warehouse);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
