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
    public class UnitOfMeasureManagementController : ControllerBase
    {
        private readonly IUnitOfMeasureManagementService _unitOfMeasureService;

        public UnitOfMeasureManagementController(IUnitOfMeasureManagementService unitOfMeasureService)
        {
            _unitOfMeasureService = unitOfMeasureService;
        }

        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<List<UnitOfMeasureDto>>> GetUnitsOfMeasureByCompany(int companyId)
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

            var units = await _unitOfMeasureService.GetUnitsOfMeasureByCompanyAsync(companyId);
            return Ok(units);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UnitOfMeasureDto>> GetUnitOfMeasure(int id)
        {
            var unit = await _unitOfMeasureService.GetUnitOfMeasureByIdAsync(id);
            if (unit == null)
            {
                return NotFound();
            }

            // Verificar que el usuario pertenezca a la empresa
            if (!User.IsInRole("Admin"))
            {
                var userCompanyId = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(userCompanyId) || int.Parse(userCompanyId) != unit.CompanyId)
                {
                    return Forbid();
                }
            }

            return Ok(unit);
        }

        [HttpPost]
        public async Task<ActionResult<UnitOfMeasureDto>> CreateUnitOfMeasure([FromBody] CreateUnitOfMeasureRequest request)
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

            var unit = await _unitOfMeasureService.CreateUnitOfMeasureAsync(request, companyId.Value);
            return CreatedAtAction(nameof(GetUnitOfMeasure), new { id = unit.Id }, unit);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UnitOfMeasureDto>> UpdateUnitOfMeasure(int id, [FromBody] CreateUnitOfMeasureRequest request)
        {
            var existingUnit = await _unitOfMeasureService.GetUnitOfMeasureByIdAsync(id);
            if (existingUnit == null)
            {
                return NotFound();
            }

            // Verificar que el usuario pertenezca a la empresa
            if (!User.IsInRole("Admin"))
            {
                var userCompanyId = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(userCompanyId) || int.Parse(userCompanyId) != existingUnit.CompanyId)
                {
                    return Forbid();
                }
            }

            var unit = await _unitOfMeasureService.UpdateUnitOfMeasureAsync(id, request);
            if (unit == null)
            {
                return NotFound();
            }

            return Ok(unit);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnitOfMeasure(int id)
        {
            var existingUnit = await _unitOfMeasureService.GetUnitOfMeasureByIdAsync(id);
            if (existingUnit == null)
            {
                return NotFound();
            }

            // Verificar que el usuario pertenezca a la empresa
            if (!User.IsInRole("Admin"))
            {
                var userCompanyId = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(userCompanyId) || int.Parse(userCompanyId) != existingUnit.CompanyId)
                {
                    return Forbid();
                }
            }

            var success = await _unitOfMeasureService.DeleteUnitOfMeasureAsync(id);
            if (!success)
            {
                return BadRequest("Cannot delete unit of measure that has associated products.");
            }

            return NoContent();
        }
    }
}
