using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Models
{
    public class ProductWarehouse
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int WarehouseId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(0, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor o igual a 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "El stock mínimo es requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El stock mínimo debe ser mayor o igual a 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinStock { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public Product? Product { get; set; }
        public Warehouse? Warehouse { get; set; }
    }
}
