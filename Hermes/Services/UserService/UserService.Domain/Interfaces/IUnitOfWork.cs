namespace UserService.Domain.Interfaces
{
    public interface IUnitOfWork<T>
    {
        void Complete();
        Task CompleteAsync();
    }
}