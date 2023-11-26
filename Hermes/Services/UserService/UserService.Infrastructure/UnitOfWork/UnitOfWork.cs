using Microsoft.EntityFrameworkCore.Storage;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Database;

namespace UserService.Infrastructure.UnitOfWork
{
    public class UnitOfWork<T> : IUnitOfWork<T>, IDisposable where T : class
    {
        private readonly HermesDbContext _context;
        private readonly IHermesRepository<T> _hermesRepository;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(HermesDbContext context, IHermesRepository<T> hermesRepository, IDbContextTransaction dbContextTransaction)
        {
            _context = context;
            _hermesRepository = hermesRepository;
            _transaction = dbContextTransaction;
        }

        public void Complete()
        {
            try
            {
                _context.SaveChanges();
                _transaction?.CommitAsync();
            }
            catch
            {
                _transaction?.RollbackAsync();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public async Task CompleteAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                _transaction?.CommitAsync();
            }
            catch
            {
                _transaction?.RollbackAsync();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
