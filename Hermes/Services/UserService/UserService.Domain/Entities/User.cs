using UserService.Domain.Exceptions;

namespace UserService.Domain.Entities
{
    public class User : Entity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string ProfileImage { get; private set; }

        public List<Interest> Interests { get; set; } = new List<Interest>();
        public List<UserFriend> Friends { get; set; } = new List<UserFriend>();
        public List<UserFollower> Followers { get; set; } = new List<UserFollower>();

        private User()
        {

        }

        public User(Guid UserId, string firstName, string lastName, DateTime dateOfBirth)
        {
            Id = UserId;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            ProfileImage = string.Empty;
        }

        public void Update(string firstName, string lastName, DateTime dateOfBirth)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }

        public void AddInterest(Interest interest)
        {
            if (!Interests.Exists(x => x.Id == interest.Id))
            {
                Interests.Add(interest);
            }
            else
            {
                throw new AlreadyExistsException("Interest already has been associated to current user.");
            }
        }

        public void RemoveInterest(Interest interest)
        {
            if (Interests.Exists(x => x.Id == interest.Id))
            {
                Interests.Remove(interest);
            }
            else
            {
                throw new AlreadyExistsException("Interest is not associated with current user.");
            }
        }

        public void AddFriend(User friend)
        {
            if (Friends.Exists(x => x.FriendId == friend.Id))
                throw new AlreadyExistsException("This user is already a friend!");

            var newFriend = new UserFriend(this, friend);

            Friends.Add(newFriend);

            UserFriend thisUser = new(friend, this);

            friend.Friends.Add(thisUser);

            AddFollower(friend);
        }

        public void RemoveFriend(Guid friendId)
        {
            var friendToRemove = Friends.FirstOrDefault(z => z.FriendId == friendId);

            Friends.Remove(friendToRemove);

            var thisUser = friendToRemove.User.Friends.FirstOrDefault(z => z.UserId == this.Id);

            friendToRemove.User.Friends.Remove(thisUser);

            RemoveFollower(friendId);
        }

        public void AddFollower(User follower)
        {
            if (!Followers.Exists(x => x.FollowerId == follower.Id))
            {
                var newFollower = new UserFollower(this, follower);

                Followers.Add(newFollower);
            }
            else
            {
                throw new AlreadyExistsException("This user is already a follower!");
            }
        }

        public void RemoveFollower(Guid followerId)
        {
            if (Followers.Exists(x => x.FollowerId == followerId))
            {
                var followerToRemove = Followers.FirstOrDefault(x => x.FollowerId == followerId);

                Followers.Remove(followerToRemove);
            }
            else
            {
                throw new NotFoundException("Follower was not found!");
            }
        }

        public void SetImageUri(string imageName)
        {
            ProfileImage = imageName;
        }

        public void RemoveImageUri()
        {
            ProfileImage = string.Empty;
        }
    }
}
