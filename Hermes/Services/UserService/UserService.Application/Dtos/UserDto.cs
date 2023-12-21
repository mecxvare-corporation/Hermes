namespace UserService.Application.Dtos
{
    public record UserMinimalInfoDto(Guid Id, string Fullname, string ProfileImage);
    public record UserDto(Guid Id, string FirstName, string LastName, DateTime DateOfBirth, string ProfileImage);
    public record CreateUserDto(string FirstName, string LastName, DateTime DateOfBirth);
    public record UpdateUserDto(Guid Id, string FirstName, string LastName, DateTime DateOfBirth);
    public record UpdateUserInterestsDto(Guid Id, List<Guid> InterestIds);
    public record DeleteUserInterestDto(Guid UserId, Guid InterestId);
    public record UserInterestsDto(UserDto User, List<InterestDto> Interests);
}