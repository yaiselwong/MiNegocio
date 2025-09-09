using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Models
{
    public class Product
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
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "El precio de venta es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de venta debe ser mayor que 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }

        public bool IsActive { get; set; } = true;

        public int CompanyId { get; set; }
        public int CategoryId { get; set; }
        public int UnitOfMeasureId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public Company? Company { get; set; }
        public Category? Category { get; set; }
        public UnitOfMeasure? UnitOfMeasure { get; set; }
        public ICollection<ProductWarehouse> ProductWarehouses { get; set; } = new List<ProductWarehouse>();
        public ICollection<ProductTransfer>? Transfers { get; set; } = new List<ProductTransfer>();
    }
}
