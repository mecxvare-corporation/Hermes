namespace PostService.Application.Dtos
{
    public record PostDto(Guid Id, Guid UserId, string Title, string Content, string? Image, DateTime CreatedDate, DateTime? UpdatedDate);
    public record CreatePostDto(Guid UserId, string Title, string Content, string? Image);
    public record UpdatePostDto(Guid Id, string Title, string Content, string? Image);
}
