using MediatR;
using UserService.Application.Dtos;
using UserService.Domain.Exceptions;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Commands
{
    public record AddUserFollowerCommand(UserFriendDto dto) : IRequest;

    public class AddUserFollowerCommandHandler : IRequestHandler<AddUserFollowerCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddUserFollowerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddUserFollowerCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(x => x.Id == request.dto.userId, true, x => x.Followers);

            if (user is null)
            {
                throw new NotFoundException("User not found!");
            }

            var follower = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(x => x.Id == request.dto.friendId, true, x => x.Followers);

            if (follower is null)
            {
                throw new NotFoundException("User not found!");
            }

            if (user.Followers.Any(x => x.Id == follower.Id))
            {
                throw new AlreadyExistsException("User is already in followers list!");
            }

            user.AddFollower(follower);

            await _unitOfWork.CompleteAsync();
        }
    }
}
