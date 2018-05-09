using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bizfitech.Banking.Api.Core.Interfaces
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> Get(Func<T, bool> expression);
        IEnumerable<T> Get(Func<T, bool> expression, string include);
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeExpressions);
        IEnumerable<T> Get(Func<T, bool> expression, Expression<Func<T, object>> include);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, object>> include);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression, Expression<Func<T, object>> include);
        Task<IEnumerable<T>> GetPageAsync(int skip, int take);
        Task<int> CountAsync();
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);
        void Add(T t);
        Task AddAsync(T t);
        void Update(T t);
        void Delete(T t);
    }
}
