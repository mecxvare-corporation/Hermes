namespace UserService.Domain.Entities
{
    public class UserFriend : Entity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid FriendId { get; set; }
        public User Friend { get; set; }

        private UserFriend()
        {

        }
        public UserFriend(User user, User friend)
        {
            User = user;
            Friend = friend;
        }
    }
}
