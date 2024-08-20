using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.DataAccess.Services.Repo
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(string? includes = null);
        Task<T> GetByIdAsync(int id, string? includes = null);
        Task<IEnumerable<T>> GetAllByFilterAsync(Expression<Func<T,bool>> predicate, string? includes=null);
        Task<T> GetAsync(Expression<Func<T,bool>> predicate,string? includes =null);
        Task CreateAsync(T obj);
        Task DeleteAsync(T obj);
        Task UpdateAsync(T obj);
    }
}
