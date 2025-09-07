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

    }
}
