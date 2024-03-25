namespace Messages
{
    public record AddUser
    {
        public Guid CommandId { get; init; }
        public Guid UserId { get; init; }
    }
}
