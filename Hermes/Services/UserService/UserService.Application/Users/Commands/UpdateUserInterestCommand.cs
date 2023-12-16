using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Dtos;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Commands
{
    public record UpdateUserInterestCommand(UpdateUserInterestsDto Dto) : IRequest;

    public class UpdateUserInterestCommandHandler : IRequestHandler<UpdateUserInterestCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserInterestCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateUserInterestCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(x => x.Id == request.Dto.Id, true, x => x.Interests);

            if (user is null)
            {
                throw new InvalidOperationException("User not found");
            }

            var interests = await _unitOfWork.InterestRepository.GetRowsQueryable(x => request.Dto.InterestIds.Contains(x.Id), true, x => x.Users).ToListAsync(cancellationToken);

            if (interests.Count == 0)
            {
                throw new InvalidOperationException("Interests not found");
            }

            foreach (var interest in interests)
            {
                user.AddInterest(interest);
            }

            await _unitOfWork.CompleteAsync();
        }
    }
}