using AutoMapper;

using MediatR;

using PostService.Application.Dtos;
using PostService.Domain.Entities;
using PostService.Domain.Interfaces;
using PostService.Infrastructure.Services;

namespace PostService.Application.Posts.Commands
{
    public record CreatePostCommand(CreatePostDto Dto, Stream? fileStream, string? fileName) : IRequest<Guid>;

    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
    {
        private readonly IPostRepository _postRepository;
        private readonly IPictureService _pictureService;
        private readonly IMapper _mapper;

        public CreatePostCommandHandler(IPostRepository postRepository, IPictureService pictureService, IMapper mapper)
        {
            _postRepository = postRepository;
            _pictureService = pictureService;
            _mapper = mapper;
        }
        public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var postEntity = _mapper.Map<Post>(request.Dto);

            if (request.fileName!= null)
            {
                Guid imageIdentifer = Guid.NewGuid();
                string imageNameToSave = (imageIdentifer.ToString() + "_" + request.fileName).ToLower();

                var imageName = await _pictureService.UploadImageAsync(request.fileStream, imageNameToSave);

                if (imageName == null)
                {
                    throw new InvalidOperationException("Image was not uploaded!");
                }

                postEntity.SetImage(imageName);
            }
            
            await _postRepository.CreateAsync(postEntity);

            return postEntity.Id;
        }
    }
}
