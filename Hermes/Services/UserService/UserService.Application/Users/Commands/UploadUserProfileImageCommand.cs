using MediatR;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Commands
{
    public record UploadUserProfilePictureCommand(Guid userId, byte[] imageData, string imageContentType) : IRequest<string>;

    public class UploadUserPictureCommandHandler : IRequestHandler<UploadUserProfilePictureCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UploadUserPictureCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(UploadUserProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var url = await _unitOfWork.UserRepository.UploadImageAsync(request.userId, request.imageData, request.imageContentType);

            return url;
        }
    }
}