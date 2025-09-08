using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; } = true;
        public int CompanyId { get; set; }
        public int WarehouseId { get; set; }
        public int CategoryId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public Company? Company { get; set; }
        public Warehouse? Warehouse { get; set; }
        public Category? Category { get; set; }
        public UnitOfMeasure? UnitOfMeasure { get; set; }
    }
}
