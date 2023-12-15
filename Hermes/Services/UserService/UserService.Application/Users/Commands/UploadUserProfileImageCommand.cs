using MediatR;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Services.ProfilePicture;

namespace UserService.Application.Users.Commands
{
    public record UploadUserProfilePictureCommand(Guid userId, Stream fileStream, string fileName) : IRequest<string>;

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
            var user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Id == request.userId, true);

            if (user == null)
            {
                throw new InvalidOperationException("User was not found!");
            }

            var imageName = await _profilePictureService.UploadImageAsync(request.fileStream, request.fileName);

            if (imageName == null) 
            {
                throw new InvalidOperationException("Image was not uploaded!");
            }

            Guid r = Guid.NewGuid();
            string userImageName = (imageName +r.ToString()).ToLower();

            user.SetImageUri(userImageName);

            await _unitOfWork.CompleteAsync();

            return userImageName;

        }
    }
}