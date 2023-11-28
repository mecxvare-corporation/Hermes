namespace UserService.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}