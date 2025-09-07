using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Data.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        void Delete(T entity);
        Task<T> Add(T entity);
        Task<T> FindOneAsync(Expression<Func<T, bool>> match);
        T Find(Expression<Func<T, bool>> match);
        void Attach(T t);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Update(T t);
        void Update2(T t, IEnumerable<T> currentItems, IEnumerable<T> currentItems2);

        Task<List<T>> GetFromSql(FormattableString query);
    }
}
