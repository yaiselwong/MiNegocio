using Microsoft.EntityFrameworkCore;
using MiNegocio.Server.Interfaces;
using MiNegocio.Shared.Data.UoW;
using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;
using MiNegocio.Shared.Models;

namespace MiNegocio.Server.Services
{
    public class UnitOfMeasureManagementService : IUnitOfMeasureManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfMeasureManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UnitOfMeasureDto>> GetUnitsOfMeasureByCompanyAsync(int companyId)
        {
            var units = await _unitOfWork.UnitOfMeasureRepository.GetAll()
                .Include(u => u.Products)
                .Where(u => u.CompanyId == companyId)
                .OrderBy(u => u.Name)
                .ToListAsync();

            return units.Select(u => new UnitOfMeasureDto
            {
                Id = u.Id,
                Name = u.Name,
                Abbreviation = u.Abbreviation,
                Description = u.Description,
                IsActive = u.IsActive,
                CompanyId = u.CompanyId,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                ProductCount = u.Products.Count
            }).ToList();
        }

        public async Task<UnitOfMeasureDto?> GetUnitOfMeasureByIdAsync(int id)
        {
            var unit = await _unitOfWork.UnitOfMeasureRepository.GetAll()
                .Include(u => u.Products)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (unit == null)
                return null;

            return new UnitOfMeasureDto
            {
                Id = unit.Id,
                Name = unit.Name,
                Abbreviation = unit.Abbreviation,
                Description = unit.Description,
                IsActive = unit.IsActive,
                CompanyId = unit.CompanyId,
                CreatedAt = unit.CreatedAt,
                UpdatedAt = unit.UpdatedAt,
                ProductCount = unit.Products.Count
            };
        }

        public async Task<UnitOfMeasureDto> CreateUnitOfMeasureAsync(CreateUnitOfMeasureRequest request, int companyId)
        {
            var unit = new UnitOfMeasure
            {
                Name = request.Name,
                Abbreviation = request.Abbreviation,
                Description = request.Description,
                CompanyId = companyId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.UnitOfMeasureRepository.Add(unit);
            await _unitOfWork.CommitAsync();

            return await GetUnitOfMeasureByIdAsync(unit.Id) ?? throw new Exception("Error creating unit of measure");
        }

        public async Task<UnitOfMeasureDto?> UpdateUnitOfMeasureAsync(int id, CreateUnitOfMeasureRequest request)
        {
            var unit = await _unitOfWork.UnitOfMeasureRepository.FindOneAsync(u => u.Id == id);

            if (unit == null)
                return null;

            unit.Name = request.Name;
            unit.Abbreviation = request.Abbreviation;
            unit.Description = request.Description;
            unit.UpdatedAt = DateTime.UtcNow;

           _unitOfWork.UnitOfMeasureRepository.Update(unit);
            await _unitOfWork.CommitAsync();

            return await GetUnitOfMeasureByIdAsync(id);
        }

        public async Task<bool> DeleteUnitOfMeasureAsync(int id)
        {
            var unit = await _unitOfWork.UnitOfMeasureRepository.GetAll()
                .Include(u => u.Products)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (unit == null)
                return false;

            // Verificar si tiene productos asociados
            if (unit.Products.Any())
            {
                return false; // No se puede eliminar si tiene productos
            }

             _unitOfWork.UnitOfMeasureRepository.Delete(unit);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
