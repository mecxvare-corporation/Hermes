using AutoMapper;
using MediatR;
using UserService.Application.Dtos;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Queries
{
    public record GetUsersQuery : IRequest<IEnumerable<UserDto>>;

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _uow.UserRepository.GetAllAsync();

            if (users.Count == 0)
            {
                throw new InvalidOperationException("No users were found!");
            }

            var userDtos = users.Select(u => _mapper.Map<UserDto>(u));

            return userDtos;
        }

    }
}
