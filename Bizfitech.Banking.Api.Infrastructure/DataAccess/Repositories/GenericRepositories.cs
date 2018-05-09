using Bizfitech.Banking.Api.Core.Interfaces;
using Bizfitech.Banking.Api.Infrastructure.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bizfitech.Banking.Api.Infrastructure.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;

        public GenericRepository(BankContext context)
        {
            _context = context;
        }

        public IEnumerable<T> Get(Func<T, bool> expression)
        {
            return _context.Set<T>().Where(expression);
        }

        public IEnumerable<T> Get(Func<T, bool> expression, string include)
        {
            return _context.Set<T>().Include(include).Where(expression);
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includeExpressions)
        {
            return includeExpressions
                .Aggregate<Expression<Func<T, object>>, IQueryable<T>>(
                _context.Set<T>(), (current, expression) => current.Include(expression));
        }

        public IEnumerable<T> Get(Func<T, bool> expression, Expression<Func<T, object>> include)
        {
            return _context.Set<T>().Include(include).Where(expression);
        }

        public virtual IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public virtual IQueryable<T> GetAll(Expression<Func<T, object>> include)
        {
            return _context.Set<T>().Include(include);
        }

        public async virtual Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>()
                                .Where(expression)
                                .ToArrayAsync();
        }

        public async virtual Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression, Expression<Func<T, object>> include)
        {
            return await _context.Set<T>()
                                .Where(expression)
                                .Include(include)
                                .ToArrayAsync();
        }

        public async Task<IEnumerable<T>> GetPageAsync(int skip, int take)
        {
            return await _context.Set<T>().Skip(skip).Take(take).ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().AnyAsync(expression);
        }

        public void Add(T t)
        {
            EntityEntry entry = _context.Entry(t);

            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                _context.Set<T>().Add(t);
            }
        }

        public async Task AddAsync(T t)
        {
            EntityEntry entry = _context.Entry(t);

            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                await _context.Set<T>().AddAsync(t);
            }
        }

        public void Update(T t)
        {
            EntityEntry entry = _context.Entry(t);

            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(t);
            }

            entry.State = EntityState.Modified;
        }

        public void Delete(T t)
        {
            EntityEntry entry = _context.Entry(t);

            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                _context.Set<T>().Attach(t);
                _context.Set<T>().Remove(t);
            }
        }
    }
}
