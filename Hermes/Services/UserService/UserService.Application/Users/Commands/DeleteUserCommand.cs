using MediatR;
using UserService.Domain.Exceptions;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Commands
{
    public record DeleteUserCommand(Guid Id) : IRequest;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {

            var user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(u => u.Id == request.Id);

            if (user == null)
            {
                throw new NotFoundException("User was not found!");
            }

            await _unitOfWork.UserRepository.DeleteAsync(user.Id);

            await _unitOfWork.CompleteAsync();
        }
    }
}
