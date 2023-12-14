using MediatR;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Services.ProfilePicture;

namespace UserService.Application.Users.Commands
{
    public record UploadUserProfilePictureCommand(Guid userId, byte[] imageData, string imageContentType) : IRequest<string>;

    public class UploadUserPictureCommandHandler : IRequestHandler<UploadUserProfilePictureCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProfilePictureService _profilePictureService;

        public UploadUserPictureCommandHandler(IUnitOfWork unitOfWork, IProfilePictureService profilePictureService)
        {
            _unitOfWork = unitOfWork;
            _profilePictureService = profilePictureService;
        }

        public async Task<string> Handle(UploadUserProfilePictureCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}