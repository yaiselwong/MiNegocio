using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;

namespace MiNegocio.Server.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetCategoriesByCompanyAsync(int companyId);
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request, int companyId);
        Task<CategoryDto?> UpdateCategoryAsync(UpdateCategoryRequest request);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
