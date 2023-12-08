namespace UserService.Application.Dtos
{
    public record InterestDto(Guid Id, string Name);
    public record DeleteUserInterestDto(Guid userId, Guid interestId);
}
