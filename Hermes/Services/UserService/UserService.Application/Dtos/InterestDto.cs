namespace UserService.Application.Dtos
{
    public record InterestDto(Guid Id, string Name);
    public record CreateInterestDto(string Name);
}
