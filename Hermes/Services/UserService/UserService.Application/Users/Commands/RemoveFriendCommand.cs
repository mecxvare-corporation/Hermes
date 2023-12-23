using MediatR;
using UserService.Application.Dtos;
using UserService.Domain.Exceptions;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Commands
{
    public record RemoveFriendCommand(UserFriendDto dto) : IRequest;

    public class RemoveFriendCommandHandler : IRequestHandler<RemoveFriendCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveFriendCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RemoveFriendCommand request, CancellationToken cancellationToken)
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

            user.RemoveFriend(friend.Id);

            await _unitOfWork.CompleteAsync();
        }
    }
}
