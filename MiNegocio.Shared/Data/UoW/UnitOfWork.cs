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

        }


        public IGenericRepository<User> UserRepository { get; set; }
        public IGenericRepository<Company> CompanyRepository { get; set; }

        public async Task<int> CommitAsync()
        {
            return await dbContext.SaveChangesAsync();
        }
    }
}
