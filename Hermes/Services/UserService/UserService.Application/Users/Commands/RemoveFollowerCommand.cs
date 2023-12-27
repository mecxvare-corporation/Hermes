using MediatR;
using UserService.Application.Dtos;
using UserService.Domain.Exceptions;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Commands
{
    public record RemoveFollowerCommand(UserFriendDto dto) : IRequest;

    public class RemoveFollowerCommandHandler : IRequestHandler<RemoveFollowerCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveFollowerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RemoveFollowerCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(x => x.Id == request.dto.userId, true, x => x.Friends);

            if (user is null)
            {
                throw new NotFoundException("User not found!");
            }

            var follower = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(x => x.Id == request.dto.friendId, true, x => x.Friends);

            if (follower is null)
            {
                throw new NotFoundException("User not found!");
            }

            user.RemoveFollower(follower.Id);

            await _unitOfWork.CompleteAsync();
        }
    }
}
