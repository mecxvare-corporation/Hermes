using UserService.Domain.Exceptions;

namespace UserService.Domain.Entities
{
    public class User : Entity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }

        public List<Interest> Interests { get; set; } = new List<Interest>();
        public List<UserFriend> Friends { get; set; } = new List<UserFriend>();
        public List<UserFollower> Followers { get; set; } = new List<UserFollower>();

        private User()
        {

        }

        public User(string firstName, string lastName, DateTime dateOfBirth)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
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
            if (!Friends.Exists(x => x.FriendId == friend.Id))
            {
                // ამ იუზერისთვის ახალი მეგობრის დამატება.
                UserFriend newFriend = new(this, friend);

                Friends.Add(newFriend);

                // მეგობრისთვის ამ იუზერის მეგობრებში ჩამატება
                UserFriend thisUser = new(friend, this);

                friend.Friends.Add(thisUser);

                AddFollower(friend);
            }
            else
            {
                throw new AlreadyExistsException("This user is already a friend!");
            }
        }

        public void RemoveFriend(User friend)
        {
            if (Friends.Exists(x => x.FriendId == friend.Id))
            {
                //ამ იუზერის მეგობრებიდან მეგობრის ამოშლა
                var friendToRemove = Friends.FirstOrDefault(z => z.FriendId == friend.Id);

                Friends.Remove(friendToRemove);

                // მეგობრის სიიდან ამ იუზერის წაშლა
                var thisUser = friend.Friends.FirstOrDefault(z => z.UserId == this.Id);

                friend.Friends.Remove(thisUser);

                RemoveFollower(friend);
            }
            else
            {
                throw new NotFoundException("Friend was not found!");
            }
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

        public void RemoveFollower(User follower)
        {
            if (Followers.Exists(x => x.FollowerId == follower.Id))
            {
                var followerToRemove = Followers.FirstOrDefault(x => x.FollowerId == follower.Id);

                Followers.Remove(followerToRemove);
            }
            else
            {
                throw new NotFoundException("Follower was not found!");
            }
        }
    }
}
