using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Dto.Request
{
    public class UpdateWarehouseRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del almacén es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "La descripción no puede exceder los 200 caracteres")]
        public string Description { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "La ubicación no puede exceder los 50 caracteres")]
        public string Address { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
