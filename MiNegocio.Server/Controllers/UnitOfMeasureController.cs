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
    public class UnitOfMeasureController : ControllerBase
    {
        private readonly IUnitOfMeasureService _unitOfMeasureService;

        public UnitOfMeasureController(IUnitOfMeasureService unitOfMeasureService)
        {
            _unitOfMeasureService = unitOfMeasureService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UnitOfMeasureDto>>> GetUnitOfMeasures()
        {
            // Obtener el CompanyId del usuario actual
            var companyIdClaim = User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId))
            {
                return BadRequest("No se pudo determinar la empresa del usuario");
            }

            var unitOfMeasures = await _unitOfMeasureService.GetUnitOfMeasuresByCompanyAsync(companyId);
            return Ok(unitOfMeasures);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UnitOfMeasureDto>> GetUnitOfMeasure(int id)
        {
            var unitOfMeasure = await _unitOfMeasureService.GetUnitOfMeasureByIdAsync(id);
            if (unitOfMeasure == null)
            {
                return NotFound();
            }

            // Verificar que la unidad de medida pertenezca a la empresa del usuario
            var companyIdClaim = User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId) || unitOfMeasure.CompanyId != companyId)
            {
                return Forbid();
            }

            return Ok(unitOfMeasure);
        }

        [HttpPost]
        public async Task<ActionResult<UnitOfMeasureDto>> CreateUnitOfMeasure([FromBody] CreateUnitOfMeasureRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Obtener el CompanyId del usuario actual
            var companyIdClaim = User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId))
            {
                return BadRequest("No se pudo determinar la empresa del usuario");
            }

            var unitOfMeasure = await _unitOfMeasureService.CreateUnitOfMeasureAsync(request, companyId);
            return CreatedAtAction(nameof(GetUnitOfMeasure), new { id = unitOfMeasure.Id }, unitOfMeasure);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UnitOfMeasureDto>> UpdateUnitOfMeasure(int id, [FromBody] UpdateUnitOfMeasureRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var unitOfMeasure = await _unitOfMeasureService.UpdateUnitOfMeasureAsync(request);
            if (unitOfMeasure == null)
            {
                return NotFound();
            }

            // Verificar que la unidad de medida pertenezca a la empresa del usuario
            var companyIdClaim = User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId) || unitOfMeasure.CompanyId != companyId)
            {
                return Forbid();
            }

            return Ok(unitOfMeasure);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnitOfMeasure(int id)
        {
            var success = await _unitOfMeasureService.DeleteUnitOfMeasureAsync(id);
            if (!success)
            {
                return BadRequest("No se puede eliminar la unidad de medida porque tiene productos asociados");
            }

            return NoContent();
        }
    }
}
