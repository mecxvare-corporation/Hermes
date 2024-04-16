using AutoMapper;

using MediatR;

using PostService.Application.Dtos;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Posts.Queries
{
    public record GetUserPostsQuery(Guid userId): IRequest<IEnumerable<PostDto>>;

    public class GetUserPostsQueryHandler : IRequestHandler<GetUserPostsQuery, IEnumerable<PostDto>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetUserPostsQueryHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostDto>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            var usersPosts = await _postRepository.GetAllAsync(request.userId);

            if (usersPosts.Count == 0)
            {
                return new List<PostDto>();
            }

            var userPosts = usersPosts.Select( p =>
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

            return userPosts;
        }
    }
}
