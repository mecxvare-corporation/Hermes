using AutoMapper;

using MediatR;

using PostService.Application.Dtos;
using PostService.Domain.Interfaces;

namespace PostService.Application.Posts.Commands
{
    public record UpdatePostCommand(UpdatePostDto postDto) : IRequest<Guid>;

    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Guid>
    {
        private readonly IPostRepository _postRepository;

        public UpdatePostCommandHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
        }

        public async Task<Guid> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetFirstOrDefaultAsync(p=>p.Id == request.postDto.Id);

            if (post == null)
            {
                throw new ArgumentNullException("Post was not found!");
            }

            post.Update(request.postDto.Title, request.postDto.Content, request.postDto.Image);

            await _postRepository.UpdateAsync(post);

            return post.Id;
        }
    }
}
