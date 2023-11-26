using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Repositories
{
    public class HermesRepository<T> : IHermesRepository<T> where T : class
    {
        private readonly DbContext _dbContext;

        public HermesRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(expression);
        }

        public async Task<List<T>> GetMultipleAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().Where(expression).ToListAsync();
        }

        public void Create(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public async void Delete(Expression<Func<T, bool>> expression)
        {
            var entity = await GetSingleAsync(expression);

            _dbContext.Set<T>().Remove(entity);
        }
    }
}
