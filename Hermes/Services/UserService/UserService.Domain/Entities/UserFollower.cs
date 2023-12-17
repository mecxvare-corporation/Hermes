namespace UserService.Domain.Entities
{
    public class UserFollower : Entity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid FollowerId { get; set; }
        public User Follower { get; set; }

        private UserFollower()
        {

        }
        public UserFollower(User user, User follower)
        {
            User = user;
            Follower = follower;
        }
    }
}
