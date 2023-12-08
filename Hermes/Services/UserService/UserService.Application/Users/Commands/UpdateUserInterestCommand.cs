using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Dtos;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Commands
{
    public record UpdateUserInterestCommand(UpdateUserInterestsDto Dto) : IRequest<Guid>;

    public class UpdateUserInterestCommandHandler : IRequestHandler<UpdateUserInterestCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserInterestCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(UpdateUserInterestCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork
                .UserRepository
                .GetFirstOrDefaultAsync(x => x.Id == request.Dto.Id, true, x => x.Interests) ?? throw new InvalidOperationException("User not found");

            var interests = await _unitOfWork.InterestRepository.GetRowsQueryable(x => request.Dto.InterestIds.Contains(x.Id), true, x => x.Users).ToListAsync();

            if (interests.Count == 0)
            {
                throw new InvalidOperationException("Interest not found");
            }

            foreach (var interest in interests)
            {
                user.AddInterest(interest);
                interest.AddUser(user);
            }

            await _unitOfWork.CompleteAsync();

            return user.Id;
        }
    }
}