using MediatR;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Services.ProfilePicture;

namespace UserService.Application.Users.Commands
{
    public record DeleteUserProfileImageCommand(Guid id) : IRequest;

    public class DeleteUserProfileImageCommandHandler : IRequestHandler<DeleteUserProfileImageCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProfilePictureService _profilePictureService;

        public DeleteUserProfileImageCommandHandler(IUnitOfWork unitOfWork, IProfilePictureService profilePictureService)
        {
            _profilePictureService = profilePictureService;
            _unitOfWork = unitOfWork;
        }

        public Task Handle(DeleteUserProfileImageCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
