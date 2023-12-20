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

            if (user.ProfileImage != string.Empty)
            {
                await _profilePictureService.DeleteImageAsync(user.ProfileImage);
                user.RemoveImageUri();
            }

            Guid imageIdentifer = Guid.NewGuid();
            string imageNameToSave = (imageIdentifer.ToString() + "_" + request.fileName).ToLower();

            var imageName = await _profilePictureService.UploadImageAsync(request.fileStream, imageNameToSave);

            if (imageName == null) 
            {
                throw new InvalidOperationException("Image was not uploaded!");
            }

            user.SetImageUri(imageName);

            await _unitOfWork.CompleteAsync();

            return imageName;
        }
    }
}