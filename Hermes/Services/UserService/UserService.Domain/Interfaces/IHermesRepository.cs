using System.Linq.Expressions;

namespace UserService.Domain.Interfaces
{
    public interface IHermesRepository<T>
    {
        Task<T> GetSingleAsync(Expression<Func<T, bool>> expression);
        Task<List<T>> GetMultipleAsync(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(Expression<Func<T, bool>> expression);
    }
}