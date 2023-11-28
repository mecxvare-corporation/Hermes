using System.Linq.Expressions;

namespace UserService.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, bool tracking, params Expression<Func<TEntity, object>>[] includes);

        public Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes);

        public IQueryable<TEntity> GetRowsQueryable(Expression<Func<TEntity, bool>> where, bool tracking, params Expression<Func<TEntity, object>>[] includes);

        public IQueryable<TEntity> GetRowsQueryable(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes);

        public Task<List<TEntity>> GetRowsAsync(Expression<Func<TEntity, bool>> where, bool tracking, params Expression<Func<TEntity, object>>[] includes);

        public Task<List<TEntity>> GetRowsAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes);

        public Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);

        public IQueryable<TEntity> GetAllQueryable(bool tracking = false);

        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> where, bool tracking = false);

        public Task<bool> AnyAsync();

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);

        public void Create(TEntity entity);

        public Task CreateRangeAsync(List<TEntity> entities);

        public void Dispose();
    }
}
