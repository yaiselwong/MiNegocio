using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Dto.Response
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
        public int CategoryId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public CategoryDto? Category { get; set; }
        public UnitOfMeasureDto? UnitOfMeasure { get; set; }
        public List<ProductWarehouseDto> ProductWarehouses { get; set; } = new();
        public decimal TotalQuantity { get; set; }
    }
}
