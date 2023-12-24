namespace UserService.Domain.Entities
{
    public class UserFriend : Entity
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public Guid FriendId { get; private set; }
        public User Friend { get; private set; }

        private UserFriend()
        {
        }

        public UserFriend(User user, User friend)
        {
            UserId = user.Id;
            User = user;
            FriendId = friend.Id;
            Friend = friend;
        }
    }
}
