using ecommerce.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ecommerce.DataAccess.Services.Repo
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext context;

        public RepositoryBase(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync(string? includes = null)
        {
            IQueryable<T> query = context.Set<T>();
            if (includes is not null)
            {
                foreach (var i in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
                return await query.ToListAsync();
            }
            return await context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByFilterAsync(Expression<Func<T, bool>> predicate, string? includes = null)
        {
            IQueryable<T> query = context.Set<T>().Where(predicate).AsQueryable();
            if(includes is not null)
            {
                foreach (var i in includes.Split(new[] {","},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
                return await query.ToListAsync();
            }
            return query;
            
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, string? includes = null)
        {
            IQueryable<T> query = context.Set<T>().Where(predicate);
            if(includes is not null)
            {
                foreach(var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetByIdAsync(int id, string? includes = null)
        {
            
            if (includes is not null)
            {
                IQueryable<T> query = context.Set<T>();
                foreach (var i in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
                return await query.FirstOrDefaultAsync();
            }

            return await context.Set<T>().FindAsync(id);

        }
        public async Task CreateAsync(T obj)
        {
            await context.Set<T>().AddAsync(obj);
        }
        public async Task DeleteAsync(T obj)
        {
            context.Set<T>().Remove(obj);
        }
        public async Task UpdateAsync(T obj)
        {
            context.Set<T>().Update(obj);
        }
        
    }
}
