using UserService.Domain.Exceptions;

namespace UserService.Domain.Entities
{
    public class User : Entity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }

        public List<Interest> Interests { get; set; } = new List<Interest>();
        public List<User> Friends { get; set; } = new List<User>();
        public List<User> Followers { get; set; } = new List<User>();

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
            if (Friends.Exists(x => x.Id == friend.Id))
                throw new AlreadyExistsException("This user is already a friend!");

            Friends.Add(friend);
            friend.Friends.Add(this);

            AddFollower(friend);
            friend.AddFollower(this);
        }

        public void RemoveFriend(Guid friendId)
        {
            var friend = Friends.FirstOrDefault(x => x.Id == friendId) ?? throw new NotFoundException("Friend was not found!");

            Friends.Remove(friend);
            friend.Friends.Remove(this);

            RemoveFollower(friend.Id);
            friend.RemoveFollower(Id);
        }

        public void AddFollower(User follower)
        {
            if (Followers.Exists(x => x.Id == follower.Id))
                throw new AlreadyExistsException("This user is already a follower!");

            Followers.Add(follower);
        }

        public void RemoveFollower(Guid followerId)
        {
            var follower = Followers.FirstOrDefault(x => x.Id == followerId) ?? throw new NotFoundException("Follower was not found!");

            Followers.Remove(follower);
        }
    }
}
