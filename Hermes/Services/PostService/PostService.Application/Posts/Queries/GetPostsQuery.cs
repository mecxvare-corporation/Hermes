using AutoMapper;

using MediatR;

using PostService.Application.Dtos;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Posts.Queries
{
    public record GetPostsQuery: IRequest<IEnumerable<PostDto>>;

    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, IEnumerable<PostDto>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetPostsQueryHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var allPosts = await _postRepository.GetAllAsync();

            if (allPosts.Count == 0)
            {
                return new List<PostDto>();
            }

            var postDtos = allPosts.Select( p =>
            {
                var postDto = _mapper.Map<Post>(p);

                return new PostDto(
                    postDto.Id,
                    postDto.UserId,
                    postDto.Title,
                    postDto.Content,
                    postDto.Image,
                    postDto.CreatedAt,
                    postDto.UpdatedAt
                    );
            });

            return postDtos;
        }
    }
}
