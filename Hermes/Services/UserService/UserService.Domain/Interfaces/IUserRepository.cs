using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
    }
}