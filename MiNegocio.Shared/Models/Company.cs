using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<UnitOfMeasure> UnitsOfMeasure { get; set; } = new List<UnitOfMeasure>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
