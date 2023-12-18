using AutoMapper;
using MediatR;
using UserService.Application.Dtos;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Services.ProfilePicture;

namespace UserService.Application.Users.Queries
{
    public record GetUsersQuery : IRequest<IEnumerable<UserDto>>;

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IProfilePictureService _profilePictureService;

        public GetUsersQueryHandler(IUnitOfWork uow, IMapper mapper, IProfilePictureService profilePictureService)
        {
            _mapper = mapper;
            _uow = uow;
            _profilePictureService = profilePictureService;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _uow.UserRepository.GetAllAsync();

            if (users == null) 
            {
                throw new ArgumentNullException("There are no users in db");
            }

            //var userDtos = users.Select(u => _mapper.Map<UserDto>(u));
            //userDtos.Select(userDto =>
            //{
            //    return new UserDto(userDto.Id, userDto.FirstName, userDto.LastName, userDto.DateOfBirth,
            //    _profilePictureService.GetImageUrl(userDto.ProfileImage));
            //}).ToList();

            var userDtos = users.Select(u =>
            {
                var userDto = _mapper.Map<UserDto>(u);
                return new UserDto(
                    userDto.Id,
                    userDto.FirstName,
                    userDto.LastName,
                    userDto.DateOfBirth,
                    _profilePictureService.GetImageUrl(userDto.ProfileImage)
                );
            }).ToList();

            return userDtos;
        }

    }
}
