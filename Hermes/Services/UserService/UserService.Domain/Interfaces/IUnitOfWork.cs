namespace UserService.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        Task<int> CompleteAsync();
    }
}