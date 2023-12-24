namespace UserService.Domain.Entities
{
    public class UserFollower : Entity
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public Guid FollowerId { get; private set; }
        public User Follower { get; private set; }

        public UserFollower()
        {
        }

        public UserFollower(User user, User follower)
        {
            UserId = user.Id;
            User = user;
            FollowerId = follower.Id;
            Follower = follower;
        }
    }
}
