using AutoMapper;
using MediatR;
using UserService.Application.Dtos;
using UserService.Domain.Exceptions;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Queries
{
    public record GetUserQuery(Guid Id) : IRequest<UserDto>;

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _uow.UserRepository.GetFirstOrDefaultAsync(u => u.Id == request.Id);

            if (user is null)
            {
                throw new NotFoundException("User not found!");
            }

            return _mapper.Map<UserDto>(user);
        }
    }
}
