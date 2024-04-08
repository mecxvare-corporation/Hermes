using AutoMapper;

using MediatR;

using PostService.Application.Dtos;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Services;

namespace PostService.Application.Posts.Commands
{
    public record UpdatePostCommand(UpdatePostDto postDto, Stream? fileStream, string? fileName) : IRequest<Guid>;

    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Guid>
    {
        private readonly IPostRepository _postRepository;
        private readonly IPictureService _pictureService;

        public UpdatePostCommandHandler(IPostRepository postRepository, IPictureService pictureService)
        {
            _postRepository = postRepository;
            _pictureService = pictureService;
        }

        public async Task<Guid> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetFirstOrDefaultAsync(p=>p.Id == request.postDto.Id);

            if (post == null)
            {
                throw new ArgumentNullException("Post was not found!");
            }

            if (request.fileName != null)
            {
                Guid imageIdentifer = Guid.NewGuid();
                string imageNameToSave = (imageIdentifer.ToString() + "_" + request.fileName).ToLower();

                var imageName = await _pictureService.UploadImageAsync(request.fileStream, imageNameToSave);

                if (imageName == null)
                {
                    throw new InvalidOperationException("Image was not uploaded!");
                }

                post.SetImage(imageName);
            }

            post.Update(request.postDto.Title, request.postDto.Content);

            await _postRepository.UpdateAsync(post);

            return post.Id;
        }
    }
}
