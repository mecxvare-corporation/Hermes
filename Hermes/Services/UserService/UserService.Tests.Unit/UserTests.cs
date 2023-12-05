using AutoMapper;
using MediatR;

namespace UserService.Tests.Unit
{
    [Collection("MyCollection")]
    public class UserTests
    {
        private readonly IServiceProvider _serviceProvider;
        public UserTests(ServiceProviderFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }
            
        [Fact]
        public void User_ConstructedWithValidData_ShouldNotThrowException()
        {
            //Arrange
            //Act
            //Assert
        }

        [Fact]
        public void User_Update_ShouldUpdateItself()
        {
            //Arrange
            //Act
            //Assert
        }
    }
}
