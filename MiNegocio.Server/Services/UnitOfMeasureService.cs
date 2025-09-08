using Microsoft.EntityFrameworkCore;
using MiNegocio.Server.Interfaces;
using MiNegocio.Shared.Data.UoW;
using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;
using MiNegocio.Shared.Models;

namespace MiNegocio.Server.Services
{
    public class UnitOfMeasureService : IUnitOfMeasureService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfMeasureService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UnitOfMeasureDto>> GetUnitOfMeasuresByCompanyAsync(int companyId)
        {
            var unitOfMeasures = await _unitOfWork.UnitOfMeasureRepository.GetAll()
                .Include(u => u.Products)
                .Where(u => u.CompanyId == companyId)
                .OrderBy(u => u.Name)
                .ToListAsync();

            return unitOfMeasures.Select(u => new UnitOfMeasureDto
            {
                Id = u.Id,
                Name = u.Name,
                Description = u.Description,
                Abbreviation = u.Abbreviation,
                IsActive = u.IsActive,
                CompanyId = u.CompanyId,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                ProductCount = u.Products.Count
            }).ToList();
        }

        public async Task<UnitOfMeasureDto?> GetUnitOfMeasureByIdAsync(int id)
        {
            var unitOfMeasure = await _unitOfWork.UnitOfMeasureRepository.GetAll()
                .Include(u => u.Products)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (unitOfMeasure == null) return null;

            return new UnitOfMeasureDto
            {
                Id = unitOfMeasure.Id,
                Name = unitOfMeasure.Name,
                Description = unitOfMeasure.Description,
                Abbreviation = unitOfMeasure.Abbreviation,
                IsActive = unitOfMeasure.IsActive,
                CompanyId = unitOfMeasure.CompanyId,
                CreatedAt = unitOfMeasure.CreatedAt,
                UpdatedAt = unitOfMeasure.UpdatedAt,
                ProductCount = unitOfMeasure.Products.Count
            };
        }

        public async Task<UnitOfMeasureDto> CreateUnitOfMeasureAsync(CreateUnitOfMeasureRequest request, int companyId)
        {
            var unitOfMeasure = new UnitOfMeasure
            {
                Name = request.Name,
                Description = request.Description,
                Abbreviation = request.Abbreviation,
                IsActive = request.IsActive,
                CompanyId = companyId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.UnitOfMeasureRepository.Add(unitOfMeasure);
            await _unitOfWork.CommitAsync();

            return await GetUnitOfMeasureByIdAsync(unitOfMeasure.Id) ?? throw new Exception("Error al crear la unidad de medida");
        }

        public async Task<UnitOfMeasureDto?> UpdateUnitOfMeasureAsync(UpdateUnitOfMeasureRequest request)
        {
            var unitOfMeasure = await _unitOfWork.UnitOfMeasureRepository.FindOneAsync(u => u.Id == request.Id);
            if (unitOfMeasure == null) return null;

            unitOfMeasure.Name = request.Name;
            unitOfMeasure.Description = request.Description;
            unitOfMeasure.Abbreviation = request.Abbreviation;
            unitOfMeasure.IsActive = request.IsActive;
            unitOfMeasure.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.UnitOfMeasureRepository.Update(unitOfMeasure);
            await _unitOfWork.CommitAsync();

            return await GetUnitOfMeasureByIdAsync(unitOfMeasure.Id);
        }

        public async Task<bool> DeleteUnitOfMeasureAsync(int id)
        {
            var unitOfMeasure = await _unitOfWork.UnitOfMeasureRepository.GetAll()
                .Include(u => u.Products)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (unitOfMeasure == null) return false;

            // Verificar si hay productos asociados
            if (unitOfMeasure.Products.Any())
            {
                return false;
            }

           _unitOfWork.UnitOfMeasureRepository.Delete(unitOfMeasure);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
