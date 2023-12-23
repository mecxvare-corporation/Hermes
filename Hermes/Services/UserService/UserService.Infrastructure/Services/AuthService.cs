using System.Diagnostics.CodeAnalysis;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Services;

public interface IAuthService
{
    Task<Guid> GetAuthorizedUserIdAsync();
}

[ExcludeFromCodeCoverage]
public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> GetAuthorizedUserIdAsync() => (await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(x => true)).Id;
}
