using AutoMapper;
using MediatR;
using UserService.Application.Dtos;
using UserService.Domain.Exceptions;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Queries
{
    public record GetUserInterestsQuery(Guid Id) : IRequest<GetUserInterestsDto>;

    public class GetUserInterestsQueryHandler : IRequestHandler<GetUserInterestsQuery, GetUserInterestsDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserInterestsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetUserInterestsDto> Handle(GetUserInterestsQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(x => x.Id == request.Id, true, x => x.Interests);

            if (user is null)
            {
                throw new NotFoundException("User not found!");
            }

            var result = new GetUserInterestsDto(_mapper.Map<UserDto>(user), _mapper.Map<List<InterestDto>>(user.Interests));

            return result;
        }
    }
}
