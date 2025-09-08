using MiNegocio.Shared.Data.Repository;
using MiNegocio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Data.UoW
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();

        IGenericRepository<User> UserRepository { get; set; }
        IGenericRepository<Company> CompanyRepository { get; set; }

         IGenericRepository<Warehouse> WarehouseRepository { get; set; }
        IGenericRepository<Category> CategoryRepository { get; set; }
        IGenericRepository<UnitOfMeasure> UnitOfMeasureRepository { get; set; }
       IGenericRepository<Product> ProductRepository { get; set; }
       IGenericRepository<ProductWarehouse> ProductWarehouseRepository { get; set; }

    }
}
