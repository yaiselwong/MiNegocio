using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Dto.Request
{
    public class CreateUnitOfMeasureRequest
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 50 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La abreviatura es requerida")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "La abreviatura debe tener entre 1 y 10 caracteres")]
        public string Abbreviation { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "La descripción no puede exceder los 200 caracteres")]
        public string Description { get; set; } = string.Empty;
    }
}
