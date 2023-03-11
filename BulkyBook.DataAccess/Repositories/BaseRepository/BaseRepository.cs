using BulkyBook.BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BulkyBook.DataAccess.Repositories.BaseRepository
{
    public class BaseRepository<TContext, TEntity> : IBaseRepository<TEntity> where TContext : DbContext where TEntity : class
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(TContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }


        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public async Task<TEntity?> Find(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if(includeFunc != null)
            {
                query = includeFunc(query);
            }    

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if(expression != null)
            {
                query = query.Where(expression);
            }

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            return await query.ToListAsync();
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<(int, IEnumerable<TEntity>)> Pagination(int page = 0,
        int pageSize = 20,
        Expression<Func<TEntity, bool>>? expression = null, 
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            var total = await query.CountAsync();

            query = query.Skip((page - 1) * pageSize)
                .Take(pageSize);

            var data = await query.ToListAsync();

            return (total, data);
        }
    }
}
