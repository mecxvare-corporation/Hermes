using AutoMapper;
using UserService.Application.Dtos;
using UserService.Domain.Entities;

namespace UserService.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            CreateMap<Interest, InterestDto>().ReverseMap();
        }
    }
}
