using PostService.Application.Dtos;

namespace PostService.Api.Models
{
    public class CreatePostRequest
    {
        public CreatePostDto Post { get; set; }
        public IFormFile? Image { get; set; }
    }
}
