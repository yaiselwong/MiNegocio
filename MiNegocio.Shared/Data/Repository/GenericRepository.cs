using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MiNegocio.Shared.Data.Repository
{
        public class GenericRepository<T> : IGenericRepository<T> where T : class
        {
            private readonly ApplicationDbContext dbContext;


            public GenericRepository(ApplicationDbContext dbContext)
            {
                this.dbContext = dbContext;
            }

            public async Task<T> Add(T entity)
            {
                var ent = await dbContext.Set<T>().AddAsync(entity);
                return ent.Entity;
            }

            public async void Delete(T entity)
            {
                //throw new NotImplementedException();
                dbContext.Set<T>().Remove(entity);
            }

            public async Task<T> FindOneAsync(Expression<Func<T, bool>> match)
            {
                return await dbContext.Set<T>().SingleOrDefaultAsync(match);
            }

            public T Find(Expression<Func<T, bool>> match)
            {
                return dbContext.Set<T>().SingleOrDefault(match);
            }

            public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
            {
                IQueryable<T> query = dbContext.Set<T>().Where(predicate);
                return query;
            }
            public IQueryable<T> GetAll()
            {
                return dbContext.Set<T>();
            }

            public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null)
            {
                var result = dbContext.Set<T>();
                if (predicate != null) return result.Where(predicate);
                return result;
            }

            public void Attach(T t)
            {
                dbContext.Set<T>().Attach(t);
            }

            public async Task<List<T>> GetAllAsync()
            {
                return await dbContext.Set<T>().ToListAsync<T>();
            }
            public async void Update(T t)
            {
                dbContext.Set<T>().Update(t);
            }
            public async void Update2(T t, IEnumerable<T> currentItems, IEnumerable<T> currentItems2)

            {
                dbContext.Set<T>().RemoveRange(currentItems);
                dbContext.Set<T>().AddRange(currentItems2);
                dbContext.Set<T>().Update(t);
            }

            public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
            {
                var result = dbContext.Set<T>();
                var response = new List<T>();
                if (predicate != null)
                    response = await result.Where(predicate).ToListAsync();
                return response;
            }

            public async Task<List<T>> GetFromSql(FormattableString query)
            {
                var result = dbContext.Set<T>().FromSqlInterpolated(query);
                var response = new List<T>();
                response = await result.ToListAsync();
                return response;
            }
        }
}
