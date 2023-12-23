using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Tests.Unit.Models
{
    [Collection("MyCollection")]
    public class UserShould
    {
        private readonly IServiceProvider _serviceProvider;

        public UserShould(ServiceProviderFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        [Fact]
        public void ConstructedWithValidData()
        {
            // Arrange
            string name = "test1";
            string lastName = "testTest";
            DateTime birthDay = DateTime.Now;

            // Act
            var newUser = new User(name, lastName, birthDay);

            // Assert
            Assert.Equal(name, newUser.FirstName);
            Assert.Equal(lastName, newUser.LastName);
            Assert.Equal(birthDay.ToString(), newUser.DateOfBirth.ToString());
        }

        [Fact]
        public void UpdateItself()
        {
            // Arrange
            string name = "test1";
            string lastName = "testTest";
            DateTime birthDay = DateTime.Now;

            var newUser = new User(name, lastName, birthDay);

            string updatedName = "test2";
            string updatedLastName = "testTest";
            DateTime updatedBirthDay = DateTime.Now;

            // Act
            newUser.Update(updatedName, updatedLastName, updatedBirthDay);

            // Assert
            Assert.Equal(updatedName, newUser.FirstName);
            Assert.Equal(updatedLastName, newUser.LastName);
            Assert.Equal(updatedBirthDay.ToString(), newUser.DateOfBirth.ToString());
        }

        [Fact]
        public void AddInterestIntoItself()
        {
            // Arrange
            string name = "test1";
            string lastName = "testTest";
            DateTime birthDay = DateTime.Now;

            var newUser = new User(name, lastName, birthDay);
            var newInterest = new Interest("Test");

            // Act
            newUser.AddInterest(newInterest);

            // Assert
            Assert.Contains(newInterest, newUser.Interests);
        }

        [Fact]
        public void NotAddSameInterestIntoItself()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var existingInterest = new Interest("Existing Interest");

            // Act
            user.AddInterest(existingInterest);

            // Assert
            Assert.Throws<AlreadyExistsException>(() => user.AddInterest(existingInterest));
        }

        [Fact]
        public void RemoveInterestFromItself()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var existingInterest = new Interest("Existing Interest");

            user.AddInterest(existingInterest);

            // Act
            user.RemoveInterest(existingInterest);

            // Assert
            Assert.DoesNotContain(existingInterest, user.Interests);
        }

        [Fact]
        public void NotRemoveAbsentInterestFromItself()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var nonExistingInterest = new Interest("Nonexisting Interest");

            // Act & Assert
            Assert.Throws<AlreadyExistsException>(() => user.RemoveInterest(nonExistingInterest));
        }

        [Fact]
        public void AddUserIntoFriendList()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var friend = new User("Takhma", "Gido", DateTime.Now);

            // Act
            user.AddFriend(friend);

            // Assert
            Assert.Contains(friend, user.Friends);
        }

        [Fact]
        public void NotAddSameUserAgainIntoFollowers()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var friend = new User("Takhma", "Gido", DateTime.Now);

            // Act
            user.AddFriend(friend);

            // Assert
            Assert.Throws<AlreadyExistsException>(() => user.AddFriend(friend));
        }

        [Fact]
        public void RemoveFriend()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var friend = new User("Takhma", "Gido", DateTime.Now);

            user.AddFriend(friend);

            // Act
            user.RemoveFriend(friend.Id);

            // Assert
            Assert.DoesNotContain(friend, user.Friends);
        }

        [Fact]
        public void AddFollower()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var follower = new User("Takhma", "Gido", DateTime.Now);

            user.AddFollower(follower);

            // Assert
            Assert.Contains(follower, user.Followers);
        }

        [Fact]
        public void NotAddSameUserAgainIntoFriends()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var follower = new User("Takhma", "Gido", DateTime.Now);

            // Act
            user.AddFollower(follower);

            // Assert
            Assert.Throws<AlreadyExistsException>(() => user.AddFriend(follower));
        }

        [Fact]
        public void RemoveFollower()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var follower = new User("Takhma", "Gido", DateTime.Now);

            user.AddFollower(follower);

            // Act
            user.RemoveFollower(follower.Id);

            // Assert
            Assert.DoesNotContain(follower, user.Followers);
        }
    }
}
