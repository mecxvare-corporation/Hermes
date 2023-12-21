using AutoMapper;
using MediatR;
using System.Diagnostics.CodeAnalysis;
using UserService.Application.Dtos;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Services;
using UserService.Infrastructure.Services.ProfilePicture;

namespace UserService.Application.Auth.Queries
{
    //TODO will be removed in future
    [ExcludeFromCodeCoverage]
    public record GetAuthorizedUserQuery() : IRequest<UserMinimalInfoDto>;

    [ExcludeFromCodeCoverage]
    public class GetAuthorizedUserQueryHandler : IRequestHandler<GetAuthorizedUserQuery, UserMinimalInfoDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IProfilePictureService _profilePictureService;
        private readonly IAuthService _authService;

        public GetAuthorizedUserQueryHandler(IUnitOfWork uow, IMapper mapper, IProfilePictureService profilePictureService, IAuthService authService)
        {
            _mapper = mapper;
            _uow = uow;
            _profilePictureService = profilePictureService;
            _authService = authService;
        }

        public async Task<UserMinimalInfoDto> Handle(GetAuthorizedUserQuery request, CancellationToken cancellationToken)
        {
            var id = await _authService.GetAuthorizedUserIdAsync(); //TODO real authorized user will be added in future

            var user = await _uow.UserRepository.GetFirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
            {
                throw new InvalidOperationException("Authorized user not found!");
            }

            var userDto = _mapper.Map<UserMinimalInfoDto>(user);

            userDto = new UserMinimalInfoDto(userDto.Id, userDto.Fullname, await _profilePictureService.GetImageUrl(userDto.ProfileImage));

            return userDto;
        }
    }
}
