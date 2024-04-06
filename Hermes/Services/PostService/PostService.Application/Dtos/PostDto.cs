namespace PostService.Application.Dtos
{
    public record PostDto(Guid Id, string Content);
    public record CreatePostDto(string Content);
}
