namespace Messages
{
    public record AddNewUser
    {
        public Guid CommandId { get; init; }
        public Guid UserId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public DateTime DateOfBirth { get; init; }

        public AddNewUser(Guid userId, string firstName, string lastName, DateTime dateOfBirth)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }
    }
}
