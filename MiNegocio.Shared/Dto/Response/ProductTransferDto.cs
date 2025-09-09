using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Dto.Response
{
    public class ProductTransferDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int FromWarehouseId { get; set; }
        public int ToWarehouseId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime TransferDate { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ProductDto? Product { get; set; }
        public WarehouseDto? FromWarehouse { get; set; }
        public WarehouseDto? ToWarehouse { get; set; }
    }
}
