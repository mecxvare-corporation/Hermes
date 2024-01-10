namespace PostService.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IPostRepository PostRepository { get; }
        Task<int> CompleteAsync();
    }
}
