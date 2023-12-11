namespace UserService.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IInterestRepository InterestRepository { get; }
        Task<int> CompleteAsync();
    }
}