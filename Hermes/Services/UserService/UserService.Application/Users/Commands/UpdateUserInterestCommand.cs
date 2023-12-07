using AutoMapper;
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
        private readonly IMapper _mapper;

        public UpdateUserInterestCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(UpdateUserInterestCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(x => x.Id == request.Dto.Id, true);

            if (user is null)
            {
                throw new InvalidOperationException("User not found");
            }

            var interests = await _unitOfWork.InterestRepository.GetQueryable(x => request.Dto.InterestIds.Contains(x.Id)).ToListAsync();

            if (interests.Count == 0)
            {
                throw new InvalidOperationException("Interest not found");
            }

            foreach (var interest in interests)
            {
                user.AddInterest(interest);//ragaca nito xdeba es sakmarisi unda iyos imdenixania ar gamomiyenebia es relacia maica aba
            }

            await _unitOfWork.CompleteAsync();

            return user.Id;
        }
    }
}