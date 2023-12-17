using MediatR;
using UserService.Application.Dtos;

namespace UserService.Application.Users.Commands
{
    public record AddUserFriendCommand(AddUserFriendDto dto) : IRequest;

    public class AddUserFriendCommandHandler : IRequestHandler<AddUserFriendCommand>
    {
        public Task Handle(AddUserFriendCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
