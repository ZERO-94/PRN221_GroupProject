using BulkyBook.BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositories.BaseRepository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        public void Add(TEntity entity);
        public void Remove(TEntity entity);
        public Task<TEntity?> Find(int id);
        public Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);
        public Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? expression = null, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeFunc = null);
    }
}
