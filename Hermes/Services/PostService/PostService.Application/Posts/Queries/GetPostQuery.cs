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
            var post = await _postRepository.GetByIdAsync(request.Id);

            if (post == null)
            {
                throw new Exception("Post Was Not Found!");
            }

            var postDto = _mapper.Map<PostDto>(post);

            return postDto;
        }
    }
}
