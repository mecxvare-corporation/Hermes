using MediatR;
using UserService.Application.Dtos;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Commands
{
    public record DeleteUserInterestCommand(DeleteUserInterestDto Dto) : IRequest;

    public class DeleteUserInterestCommandHandler : IRequestHandler<DeleteUserInterestCommand>
    {
        private readonly IUnitOfWork _uow;

        public DeleteUserInterestCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Handle(DeleteUserInterestCommand request, CancellationToken cancellationToken)
        {
            var user = await _uow.UserRepository.GetFirstOrDefaultAsync(x => x.Id == request.Dto.userId, true, x => x.Interests)
                ?? throw new InvalidOperationException($"User with Id {request.Dto.userId} not found");

            var interest = await _uow.InterestRepository.GetFirstOrDefaultAsync(x => x.Id == request.Dto.interestId, true, x => x.Users)
                ?? throw new InvalidOperationException($"Interest with Id {request.Dto.interestId} not found");

            user.RemoveInterest(interest);

            await _uow.CompleteAsync();
        }
    }
}
