using AutoMapper;
using MediatR;
using UserService.Application.Dtos;
using UserService.Domain.Exceptions;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Queries
{
    public record GetUserFriendsQuery(Guid id) : IRequest<GetUserFriendsDto>;

    public class GetUserFriendsQueryHandler : IRequestHandler<GetUserFriendsQuery, GetUserFriendsDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserFriendsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetUserFriendsDto> Handle(GetUserFriendsQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetFirstOrDefaultAsync(x => x.Id == request.id, true, x => x.Friends);

            if (user is null)
            {
                throw new NotFoundException("User not found!");
            }

            var result = new GetUserFriendsDto(_mapper.Map<UserDto>(user), _mapper.Map<List<UserDto>>(user.Friends));

            return result;
        }
    }
}
