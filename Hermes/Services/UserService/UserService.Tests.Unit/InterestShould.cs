using UserService.Domain.Entities;

namespace UserService.Tests.Unit
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

            var newUser = new User(name, lastName, birthDay);

            //Act
            var newInterest = new Interest(interestName);
            newInterest.AddUser(newUser);

            //Assert
            Assert.Contains(newUser, newInterest.Users);
        }
    }
}
