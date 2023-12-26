using MediatR;
using UserService.Application.Dtos;
using UserService.Domain.Exceptions;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Commands
{
    public record AddUserFriendCommand(UserFriendDto dto) : IRequest;

    public class AddUserFriendCommandHandler : IRequestHandler<AddUserFriendCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddUserFriendCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddUserFriendCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(x => x.Id == request.dto.userId, true, x => x.Friends);

            if (user is null)
            {
                throw new NotFoundException("User not found!");
            }

            var friend = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(x => x.Id == request.dto.friendId, true, x => x.Friends);

            if (friend is null)
            {
                throw new NotFoundException("User not found!");
            }

            if (user.Friends.Any(x => x.Friend.Id == friend.Id))
            {
                throw new AlreadyExistsException("User is already in friend list!");
            }

            user.AddFriend(friend);

            await _unitOfWork.CompleteAsync();
        }
    }
}
