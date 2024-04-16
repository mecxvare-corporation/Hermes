using MediatR;

using PostService.Domain.Interfaces;

namespace PostService.Application.Posts.Commands
{
    public record DeletePostCommand(Guid Id): IRequest;

    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
    {
        private readonly IPostRepository _postRepository;
        public DeletePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var result = await _postRepository.DeleteAsync(request.Id);
            if (!result)
            {
                throw new ArgumentException("Not Found!");
            }
        }
    }
}
