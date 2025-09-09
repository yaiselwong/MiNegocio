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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IWarehouseService _warehouseService;

        public ProductController(IProductService productService, IWarehouseService warehouseService)
        {
            _productService = productService;
            _warehouseService = warehouseService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetProducts()
        {
            // Obtener el CompanyId del usuario actual
            var companyIdClaim = User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId))
            {
                return BadRequest("No se pudo determinar la empresa del usuario");
            }

            var products = await _productService.GetProductsByCompanyAsync(companyId);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Verificar que el producto pertenezca a la empresa del usuario
            var companyIdClaim = User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId) || product.CompanyId != companyId)
            {
                return Forbid();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductRequest request)
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

            var product = await _productService.CreateProductAsync(request, companyId);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            request.Id = id; // Asegurar que el ID coincida

            var product = await _productService.UpdateProductAsync(request);
            if (product == null)
            {
                return NotFound();
            }

            // Verificar que el producto pertenezca a la empresa del usuario
            var companyIdClaim = User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId) || product.CompanyId != companyId)
            {
                return Forbid();
            }

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("warehouse/{id}")]
        public async Task<ActionResult<ProductWarehouseDto>> UpdateProductWarehouse(int id, [FromBody] UpdateProductWarehouseRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            request.Id = id; // Asegurar que el ID coincida

            var productWarehouse = await _productService.UpdateProductWarehouseAsync(request);
            if (productWarehouse == null)
            {
                return NotFound();
            }

            return Ok(productWarehouse);
        }

        [HttpGet("{productId}/warehouses")]
        public async Task<ActionResult<List<ProductWarehouseDto>>> GetProductWarehouses(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            // Verificar que el producto pertenezca a la empresa del usuario
            var companyIdClaim = User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId) || product.CompanyId != companyId)
            {
                return Forbid();
            }

            var productWarehouses = await _productService.GetProductWarehousesAsync(productId);
            return Ok(productWarehouses);
        }
        [HttpPost("transfer")]
        public async Task<IActionResult> TransferProduct([FromBody] CreateProductTransferRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar que el producto pertenezca a la empresa del usuario
            var companyIdClaim = User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId))
            {
                return BadRequest("No se pudo determinar la empresa del usuario");
            }

            var product = await _productService.GetProductByIdAsync(request.ProductId);
            if (product == null || product.CompanyId != companyId)
            {
                return Forbid();
            }

            // Verificar que los almacenes pertenezcan a la empresa
            var warehouses = await _warehouseService.GetWarehousesByCompanyAsync(companyId);
            var fromWarehouse = warehouses.FirstOrDefault(w => w.Id == request.FromWarehouseId);
            var toWarehouse = warehouses.FirstOrDefault(w => w.Id == request.ToWarehouseId);

            if (fromWarehouse == null || toWarehouse == null ||
                fromWarehouse.CompanyId != companyId || toWarehouse.CompanyId != companyId)
            {
                return Forbid();
            }

            // Verificar que no sea el mismo almacén
            if (request.FromWarehouseId == request.ToWarehouseId)
            {
                return BadRequest("No se puede transferir al mismo almacén");
            }

            try
            {
                var success = await _productService.TransferProductAsync(request);
                if (success)
                {
                    return Ok(new { message = "Transferencia realizada exitosamente" });
                }
                return BadRequest("Error al realizar la transferencia");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{productId}/transfers")]
        public async Task<ActionResult<List<ProductTransferDto>>> GetProductTransfers(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            // Verificar que el producto pertenezca a la empresa del usuario
            var companyIdClaim = User.FindFirst("CompanyId")?.Value;
            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int companyId) || product.CompanyId != companyId)
            {
                return Forbid();
            }

            var transfers = await _productService.GetProductTransfersAsync(productId);
            return Ok(transfers);
        }
    }
}
