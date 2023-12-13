using System.Diagnostics.CodeAnalysis;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Database;
using UserService.Infrastructure.Services.ProfilePicture;

namespace UserService.Infrastructure.Repositories
{
    [ExcludeFromCodeCoverage]
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IProfilePictureService _service;

        public UserRepository(UserDbContext dbContext, IProfilePictureService service) : base(dbContext)
        {
            _service = service;
        }

        public Task<string> UploadImageAsync(Guid userId, byte[] imageData, string imageContentType)
        {
            throw new NotImplementedException();
        }
    }
}
