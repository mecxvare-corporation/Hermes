using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Database;

using System.Linq.Expressions;

namespace PostService.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly PostsStoreDatabase _context;

        public Repository(PostsStoreDatabase context)
        {
            _context = context;
        }

        public Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includes)
        {
            throw new NotImplementedException();
        }
    }
}
