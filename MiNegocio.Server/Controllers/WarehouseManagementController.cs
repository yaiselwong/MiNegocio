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
    public class WarehouseManagementController : ControllerBase
    {
        private readonly IWarehouseManagementService _warehouseService;

        public WarehouseManagementController(IWarehouseManagementService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<List<WarehouseDto>>> GetWarehousesByCompany(int companyId)
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

            // Verificar que el usuario pertenezca a la empresa
            if (!User.IsInRole("Admin"))
            {
                var userCompanyId = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(userCompanyId) || int.Parse(userCompanyId) != warehouse.CompanyId)
                {
                    return Forbid();
                }
            }

            return Ok(warehouse);
        }

        [HttpPost]
        public async Task<ActionResult<WarehouseDto>> CreateWarehouse([FromBody] CreateWarehouseRequest request)
        {
            // Obtener el companyId del usuario
            int? companyId = null;
            if (!User.IsInRole("Admin"))
            {
                companyId = int.Parse(User.FindFirst("CompanyId")?.Value ?? "0");
            }
            else
            {
                // Para admins, el companyId debe venir en el request o usar el primero disponible
                companyId = 1; // Por ahora, esto debería venir del frontend
            }

            if (!companyId.HasValue || companyId == 0)
            {
                return BadRequest("Company ID is required");
            }

            var warehouse = await _warehouseService.CreateWarehouseAsync(request, companyId.Value);
            return CreatedAtAction(nameof(GetWarehouse), new { id = warehouse.Id }, warehouse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WarehouseDto>> UpdateWarehouse(int id, [FromBody] CreateWarehouseRequest request)
        {
            var existingWarehouse = await _warehouseService.GetWarehouseByIdAsync(id);
            if (existingWarehouse == null)
            {
                return NotFound();
            }

            // Verificar que el usuario pertenezca a la empresa
            if (!User.IsInRole("Admin"))
            {
                var userCompanyId = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(userCompanyId) || int.Parse(userCompanyId) != existingWarehouse.CompanyId)
                {
                    return Forbid();
                }
            }

            var warehouse = await _warehouseService.UpdateWarehouseAsync(id, request);
            if (warehouse == null)
            {
                return NotFound();
            }

            return Ok(warehouse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            var existingWarehouse = await _warehouseService.GetWarehouseByIdAsync(id);
            if (existingWarehouse == null)
            {
                return NotFound();
            }

            // Verificar que el usuario pertenezca a la empresa
            if (!User.IsInRole("Admin"))
            {
                var userCompanyId = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(userCompanyId) || int.Parse(userCompanyId) != existingWarehouse.CompanyId)
                {
                    return Forbid();
                }
            }

            var success = await _warehouseService.DeleteWarehouseAsync(id);
            if (!success)
            {
                return BadRequest("Cannot delete warehouse that has associated products.");
            }

            return NoContent();
        }
    }
}
