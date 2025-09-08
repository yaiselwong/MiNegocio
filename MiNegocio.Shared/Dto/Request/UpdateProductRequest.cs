using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Dto.Request
{
    public class UpdateProductRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "La descripción no puede exceder los 200 caracteres")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código del producto es requerido")]
        [StringLength(50, ErrorMessage = "El código no puede exceder los 50 caracteres")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio de compra es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de compra debe ser mayor que 0")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "El precio de venta es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de venta debe ser mayor que 0")]
        public decimal SalePrice { get; set; }

        [Required(ErrorMessage = "La categoría es requerida")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "La unidad de medida es requerida")]
        public int UnitOfMeasureId { get; set; }

        public bool IsActive { get; set; } = true;

        // Lista de almacenes con cantidades actualizadas
        public List<ProductWarehouseRequest> Warehouses { get; set; } = new();
    }
}
