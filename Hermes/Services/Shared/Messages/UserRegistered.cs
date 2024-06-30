namespace Messages
{
    public class UserRegistered
    {
        public Guid UserId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public DateTime DateOfBirth { get; init; }
    }
}
