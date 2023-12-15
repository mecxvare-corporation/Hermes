using MediatR;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Services.ProfilePicture;

namespace UserService.Application.Users.Commands
{
    public record DeleteUserProfileImageCommand(Guid userId) : IRequest;

    public class DeleteUserProfileImageCommandHandler : IRequestHandler<DeleteUserProfileImageCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProfilePictureService _profilePictureService;

        public DeleteUserProfileImageCommandHandler(IUnitOfWork unitOfWork, IProfilePictureService profilePictureService)
        {
            _profilePictureService = profilePictureService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteUserProfileImageCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Id == request.userId, true);

            if (user == null)
            {
                throw new InvalidOperationException("User was not found!");
            }

            await _profilePictureService.DeleteImageAsync(user.ProfileImage);

            user.RemoveImageUri();

            await _unitOfWork.CompleteAsync();
        }
    }
}
