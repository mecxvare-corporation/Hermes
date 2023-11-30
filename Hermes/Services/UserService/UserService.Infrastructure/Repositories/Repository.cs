using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Repositories
{
    public abstract class Repository<TEntity> where TEntity : Entity
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, bool tracking, params Expression<Func<TEntity, object>>[] includes)
        {
            var queryable = GetQueryable(where, tracking);

            foreach (var includeExpression in includes)
            {
                queryable = queryable.Include(includeExpression);
            }

            return await queryable.FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
        {
            return await GetFirstOrDefaultAsync(where, false, includes);
        }

        public IQueryable<TEntity> GetRowsQueryable(Expression<Func<TEntity, bool>> where, bool tracking, params Expression<Func<TEntity, object>>[] includes)
        {
            var queryable = GetQueryable(where, tracking);

            foreach (var include in includes)
            {
                queryable = queryable.Include(include);
            }

            return queryable;
        }

        public IQueryable<TEntity> GetRowsQueryable(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
        {
            return GetRowsQueryable(where, false, includes);
        }

        public async Task<List<TEntity>> GetRowsAsync(Expression<Func<TEntity, bool>> where, bool tracking, params Expression<Func<TEntity, object>>[] includes)
        {
            var queryable = GetRowsQueryable(where, tracking, includes);

            return await queryable.ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetRowsAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
        {
            return await GetRowsAsync(where, false, includes);
        }

        public virtual async Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            var queryable = GetAllQueryable();

            foreach (var includeExpression in includes)
            {
                queryable = queryable.Include(includeExpression);
            }

            return await queryable.ToListAsync();
        }

        public virtual IQueryable<TEntity> GetAllQueryable(bool tracking = false)
        {
            IQueryable<TEntity> queryable = _dbContext.Set<TEntity>().AsQueryable();

            return tracking ? queryable : queryable.AsNoTracking();
        }

        public virtual IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> where, bool tracking = false)
        {
            return GetAllQueryable(tracking).Where(where);
        }

        public async Task<bool> AnyAsync()
        {
            return await _dbContext.Set<TEntity>().AnyAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbContext.Set<TEntity>().AnyAsync(expression);
        }

        public virtual void Create(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Set<TEntity>().Add(entity);
        }

        public async Task CreateRangeAsync(List<TEntity> entities)
        {
            await _dbContext.AddRangeAsync(entities);
        }

        public async Task DeleteAsync(Guid id) 
        {
            var entity = await GetFirstOrDefaultAsync(u => u.Id == id);

            if (entity == null) 
            {
                throw new InvalidOperationException($"{typeof(TEntity).Name} With Id: {id}, Does not exists!");
            }

            _dbContext.Set<TEntity>().Remove(entity);
        }

        public virtual void Dispose()
        {
            _dbContext.DisposeAsync();
        }
    }
}
