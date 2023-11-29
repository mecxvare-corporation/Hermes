namespace UserService.Application.Dtos
{
    public record UserDto(Guid Id, string FirstName, string LastName, DateTime DateOfBirth);
    public record CreateUserDto(string FirstName, string LastName, DateTime DateOfBirth);
    public record UpdateUserDto(Guid Id, string FirstName, string LastName, DateTime DateOfBirth);
}
