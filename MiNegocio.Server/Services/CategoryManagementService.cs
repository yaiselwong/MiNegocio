using Microsoft.EntityFrameworkCore;
using MiNegocio.Server.Interfaces;
using MiNegocio.Shared.Data.UoW;
using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;
using MiNegocio.Shared.Models;

namespace MiNegocio.Server.Services
{
    public class CategoryManagementService : ICategoryManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CategoryDto>> GetCategoriesByCompanyAsync(int companyId)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAll()
                .Include(c => c.Products)
                .Where(c => c.CompanyId == companyId)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive,
                CompanyId = c.CompanyId,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                ProductCount = c.Products.Count
            }).ToList();
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetAll()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CompanyId = category.CompanyId,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
                ProductCount = category.Products.Count
            };
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request, int companyId)
        {
            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                CompanyId = companyId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.CategoryRepository.Add(category);
            await _unitOfWork.CommitAsync();

            return await GetCategoryByIdAsync(category.Id) ?? throw new Exception("Error creating category");
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(int id, CreateCategoryRequest request)
        {
            var category = await _unitOfWork.CategoryRepository.FindOneAsync(c => c.Id == id);

            if (category == null)
                return null;

            category.Name = request.Name;
            category.Description = request.Description;
            category.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.CommitAsync();

            return await GetCategoryByIdAsync(id);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetAll()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return false;

            // Verificar si tiene productos asociados
            if (category.Products.Any())
            {
                return false; // No se puede eliminar si tiene productos
            }

             _unitOfWork.CategoryRepository.Delete(category);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }
}
