using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<string> UploadImageAsync(Guid userId, byte[] imageData, string imageContentType);
    }
}