using AutoMapper;

using MediatR;

using PostService.Application.Dtos;
using PostService.Domain.Interfaces;

namespace PostService.Application.Posts.Queries
{
    public record GetPostQuery(Guid Id) : IRequest<PostDto>;

    public class GetPostQueryHandler : IRequestHandler<GetPostQuery, PostDto>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetPostQueryHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }
        public async Task<PostDto> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetFirstOrDefaultAsync(p=>p.Id == request.Id);

            if (post == null)
            {
                throw new Exception("Post Was Not Found!");
            }

            var postDto = new PostDto(
                    post.Id,
                    post.UserId,
                    post.Title,
                    post.Content,
                    post.Image,
                    post.CreatedAt,
                    post.UpdatedAt
                    );

            return postDto;
        }
    }
}
