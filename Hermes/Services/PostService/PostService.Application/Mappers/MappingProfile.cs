using AutoMapper;

using PostService.Application.Dtos;
using PostService.Domain.Entities;

namespace PostService.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<CreatePostDto, Post>();
        }
    }
}
