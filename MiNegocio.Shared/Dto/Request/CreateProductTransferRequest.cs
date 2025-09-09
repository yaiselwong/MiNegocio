using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Dto.Request
{
    public class CreateProductTransferRequest
    {
        [Required(ErrorMessage = "El producto es requerido")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "El almacén de origen es requerido")]
        public int FromWarehouseId { get; set; }

        [Required(ErrorMessage = "El almacén de destino es requerido")]
        public int ToWarehouseId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
        public decimal Quantity { get; set; }

        [StringLength(200, ErrorMessage = "Las notas no pueden exceder los 200 caracteres")]
        public string? Notes { get; set; }

        public DateTime TransferDate { get; set; } = DateTime.UtcNow;
    }
}
