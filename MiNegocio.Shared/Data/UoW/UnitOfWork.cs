using MiNegocio.Shared.Data.Repository;
using MiNegocio.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

            UserRepository = UserRepository ?? new GenericRepository<User>(dbContext);
            CompanyRepository = CompanyRepository ?? new GenericRepository<Company>(dbContext);

            WarehouseRepository = new GenericRepository<Warehouse>(dbContext);
            CategoryRepository = new GenericRepository<Category>(dbContext);
            UnitOfMeasureRepository = new GenericRepository<UnitOfMeasure>(dbContext);
            ProductRepository = new GenericRepository<Product>(dbContext);
            ProductWarehouseRepository = new GenericRepository<ProductWarehouse>(dbContext);

        }


        public IGenericRepository<User> UserRepository { get; set; }
        public IGenericRepository<Company> CompanyRepository { get; set; }

        public IGenericRepository<Warehouse> WarehouseRepository { get; set; }
        public IGenericRepository<Category> CategoryRepository { get; set; }
        public IGenericRepository<UnitOfMeasure> UnitOfMeasureRepository { get; set; }
        public IGenericRepository<Product> ProductRepository { get; set; }
        public IGenericRepository<ProductWarehouse> ProductWarehouseRepository { get; set; }

        public async Task<int> CommitAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
