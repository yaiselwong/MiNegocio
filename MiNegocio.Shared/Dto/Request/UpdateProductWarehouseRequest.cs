using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Dto.Request
{
    public class UpdateProductWarehouseRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(0, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor o igual a 0")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "El stock mínimo es requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El stock mínimo debe ser mayor o igual a 0")]
        public decimal MinStock { get; set; }
    }
}
