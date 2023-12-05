using UserService.Domain.Entities;

namespace UserService.Tests.Unit
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
            //Arrange
            string name = "test1";
            string lastName = "testTest";
            DateTime birthDay = DateTime.Now;

            //Act
            var newUser = new User(name, lastName, birthDay);

            //Assert
            Assert.Equal(name, newUser.FirstName);
            Assert.Equal(lastName, newUser.LastName);
            Assert.Equal(birthDay.ToString(), newUser.DateOfBirth.ToString());
        }

        [Fact]
        public void UpdateItself()
        {
            //Arrange
            string name = "test1";
            string lastName = "testTest";
            DateTime birthDay = DateTime.Now;

            var newUser = new User(name, lastName, birthDay);

            string updatedName = "test2";
            string updatedLastName = "testTest";
            DateTime updatedBirthDay = DateTime.Now;

            //Act
            newUser.Update(updatedName, updatedLastName, updatedBirthDay);

            //Assert
            Assert.Equal(updatedName, newUser.FirstName);
            Assert.Equal(updatedLastName, newUser.LastName);
            Assert.Equal(updatedBirthDay.ToString(), newUser.DateOfBirth.ToString());
        }
    }
}
