using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Models
{
    public class ProductTransfer
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int FromWarehouseId { get; set; }
        public int ToWarehouseId { get; set; }

        [Required(ErrorMessage = "La cantidad a transferir es requerida")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }

        public DateTime TransferDate { get; set; }

        [StringLength(200, ErrorMessage = "Las notas no pueden exceder los 200 caracteres")]
        public string? Notes { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Completed, Cancelled

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public Product? Product { get; set; }
        public Warehouse? FromWarehouse { get; set; }
        public Warehouse? ToWarehouse { get; set; }
    }
}
