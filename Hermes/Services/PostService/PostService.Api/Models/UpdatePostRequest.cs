using PostService.Application.Dtos;

namespace PostService.Api.Models
{
    public class UpdatePostRequest
    {
        public UpdatePostDto Post { get; set; }
        public IFormFile? Image { get; set; }
    }
}
