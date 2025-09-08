using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Dto.Request
{

    public class UpdateUnitOfMeasureRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la unidad es requerido")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "La descripción no puede exceder los 100 caracteres")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "La abreviatura es requerida")]
        [StringLength(10, ErrorMessage = "La abreviatura no puede exceder los 10 caracteres")]
        public string Abbreviation { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
