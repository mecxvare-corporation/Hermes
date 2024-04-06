using AutoMapper;

using MediatR;

using PostService.Application.Dtos;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;

namespace PostService.Application.Posts.Commands
{
    public record CreatePostCommand(CreatePostDto Dto) : IRequest<Guid>;

    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public CreatePostCommandHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }
        public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = _mapper.Map<Post>(request.Dto);

            _postRepository.Create(post);

            return post.Id;
        }
    }
}
