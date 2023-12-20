using AutoMapper;
using MediatR;
using UserService.Application.Dtos;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Services.ProfilePicture;

namespace UserService.Application.Users.Queries
{
    public record GetUserQuery(Guid Id) : IRequest<UserDto>;

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IProfilePictureService _profilePictureService;

        public GetUserQueryHandler(IUnitOfWork uow, IMapper mapper, IProfilePictureService profilePictureService)
        {
            _mapper = mapper;
            _uow = uow;
            _profilePictureService = profilePictureService;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _uow.UserRepository.GetFirstOrDefaultAsync(u => u.Id == request.Id);

            if (user is null)
            {
                throw new InvalidOperationException("User not found!");
            }

            var userDto = _mapper.Map<UserDto>(user);
            userDto = new UserDto(userDto.Id, userDto.FirstName, userDto.LastName, userDto.DateOfBirth,
                 await _profilePictureService.GetImageUrl(userDto.ProfileImage));

            return userDto;
        }
    }
}
