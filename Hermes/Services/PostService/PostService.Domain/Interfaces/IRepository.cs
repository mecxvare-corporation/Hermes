using PostService.Domain.Entities;

using System.Linq.Expressions;

namespace PostService.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        public Task<TEntity> CreateAsync(TEntity entity);
        public Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter);
        public Task<List<TEntity>> GetAllAsync();
        public Task<List<TEntity>> GetAllAsync(Guid userId);
        public Task<bool> UpdateAsync(TEntity entity);
        public Task<bool> DeleteAsync(Guid id);
    }
}
