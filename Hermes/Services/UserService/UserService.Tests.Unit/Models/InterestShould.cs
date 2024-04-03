using UserService.Domain.Entities;

namespace UserService.Tests.Unit.Models
{
    [Collection("MyCollection")]
    public class InterestShould
    {
        private readonly IServiceProvider _serviceProvider;

        public InterestShould(ServiceProviderFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        [Fact]
        public void ConstructedWithValidData()
        {
            //Arrange
            string interestName = "TestName";

            //Act
            var newInterest = new Interest(interestName);

            //Assert
            Assert.Equal(interestName, newInterest.Name);
        }

        [Fact]
        public void AddUserToItself()
        {
            //Arrange
            string interestName = "TestName";

            string name = "test1";
            string lastName = "testTest";
            DateTime birthDay = DateTime.Now;

            var newInterest = new Interest(interestName);
            var newUser = new User(Guid.NewGuid(), name, lastName, birthDay);

            //Act
            newInterest.AddUser(newUser);

            //Assert
            Assert.Contains(newUser, newInterest.Users);
        }

        [Fact]
        public void NotAddSameUserIntoItself()
        {
            // Arrange
            var interest = new Interest("Interest");

            var user = new User(Guid.NewGuid(), "Esgeso", "Namoradze", DateTime.Now);

            // Act
            interest.AddUser(user);

            // Assert
            Assert.Throws<InvalidOperationException>(() => interest.AddUser(user));
        }
    }
}
