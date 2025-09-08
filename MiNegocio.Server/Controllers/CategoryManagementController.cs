using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiNegocio.Server.Interfaces;
using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;
using MiNegocio.Shared.Models;

namespace MiNegocio.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryManagementController : ControllerBase
    {
        private readonly ICategoryManagementService _categoryService;

        public CategoryManagementController(ICategoryManagementService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<List<CategoryDto>>> GetCategoriesByCompany(int companyId)
        {
            // Verificar que el usuario pertenezca a la empresa
            if (!User.IsInRole("Admin"))
            {
                var userCompanyId = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(userCompanyId) || int.Parse(userCompanyId) != companyId)
                {
                    return Forbid();
                }
            }

            var categories = await _categoryService.GetCategoriesByCompanyAsync(companyId);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            // Verificar que el usuario pertenezca a la empresa
            if (!User.IsInRole("Admin"))
            {
                var userCompanyId = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(userCompanyId) || int.Parse(userCompanyId) != category.CompanyId)
                {
                    return Forbid();
                }
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            // Obtener el companyId del usuario
            int? companyId = null;
            if (!User.IsInRole("Admin"))
            {
                companyId = int.Parse(User.FindFirst("CompanyId")?.Value ?? "0");
            }
            else
            {
                // Para admins, el companyId debe venir del frontend
                companyId = 1; // Por ahora, esto debería venir del frontend
            }

            if (!companyId.HasValue || companyId == 0)
            {
                return BadRequest("Company ID is required");
            }

            var category = await _categoryService.CreateCategoryAsync(request, companyId.Value);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] CreateCategoryRequest request)
        {
            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            // Verificar que el usuario pertenezca a la empresa
            if (!User.IsInRole("Admin"))
            {
                var userCompanyId = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(userCompanyId) || int.Parse(userCompanyId) != existingCategory.CompanyId)
                {
                    return Forbid();
                }
            }

            var category = await _categoryService.UpdateCategoryAsync(id, request);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            // Verificar que el usuario pertenezca a la empresa
            if (!User.IsInRole("Admin"))
            {
                var userCompanyId = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(userCompanyId) || int.Parse(userCompanyId) != existingCategory.CompanyId)
                {
                    return Forbid();
                }
            }

            var success = await _categoryService.DeleteCategoryAsync(id);
            if (!success)
            {
                return BadRequest("Cannot delete category that has associated products.");
            }

            return NoContent();
        }
    }
}
