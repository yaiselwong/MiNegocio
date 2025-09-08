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
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet]
        public async Task<ActionResult<List<WarehouseDto>>> GetWarehouses()
        {
            // Obtener el CompanyId del usuario actual
            var companyIdClaim = User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId))
            {
                return BadRequest("No se pudo determinar la empresa del usuario");
            }

            var warehouses = await _warehouseService.GetWarehousesByCompanyAsync(companyId);
            return Ok(warehouses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WarehouseDto>> GetWarehouse(int id)
        {
            var warehouse = await _warehouseService.GetWarehouseByIdAsync(id);
            if (warehouse == null)
            {
                return NotFound();
            }

            // Verificar que el almacén pertenezca a la empresa del usuario
            var companyIdClaim = User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId) || warehouse.CompanyId != companyId)
            {
                return Forbid();
            }

            return Ok(warehouse);
        }

        [HttpPost]
        public async Task<ActionResult<WarehouseDto>> CreateWarehouse([FromBody] CreateWarehouseRequest request)
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

            var warehouse = await _warehouseService.CreateWarehouseAsync(request, companyId);
            return CreatedAtAction(nameof(GetWarehouse), new { id = warehouse.Id }, warehouse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WarehouseDto>> UpdateWarehouse(int id, [FromBody] UpdateWarehouseRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var warehouse = await _warehouseService.UpdateWarehouseAsync(request);
            if (warehouse == null)
            {
                return NotFound();
            }

            // Verificar que el almacén pertenezca a la empresa del usuario
            var companyIdClaim = User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId) || warehouse.CompanyId != companyId)
            {
                return Forbid();
            }

            return Ok(warehouse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            var success = await _warehouseService.DeleteWarehouseAsync(id);
            if (!success)
            {
                return BadRequest("No se puede eliminar el almacén porque tiene productos asociados");
            }

            return NoContent();
        }
    }
}
