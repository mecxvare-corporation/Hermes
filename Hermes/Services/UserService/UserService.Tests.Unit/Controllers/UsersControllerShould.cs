using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.Api.Controllers;
using UserService.Application.Dtos;
using UserService.Application.Users.Queries;

namespace UserService.Tests.Unit.Controllers
{
    [Collection("MyCollection")]
    public class UsersControllerShould
    {
        private readonly IServiceProvider _serviceProvider;

        public UsersControllerShould(ServiceProviderFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        [Fact]
        public async Task GetAllUsers()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var controller = new UsersController(mediatorMock.Object);

            // Act
            var result = await controller.GetUsersAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var finalResult = Assert.IsAssignableFrom<IEnumerable<UserDto>>(okResult.Value);
        }

        [Fact]
        public async Task GetSingleUserById()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();

            var userDtoToReturn = new UserDto(Guid.NewGuid(), "Esgeso", "Namoradze", DateTime.Now, "test");

            mediatorMock.Setup(m => m.Send(It.IsAny<GetUserQuery>(), default))
                .ReturnsAsync(userDtoToReturn);

            var controller = new UsersController(mediatorMock.Object);

            // Act
            var result = await controller.GetUserAsync(userDtoToReturn.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var userDto = Assert.IsAssignableFrom<UserDto>(okResult.Value);

            Assert.NotNull(userDto);
            Assert.Equal(userDtoToReturn.Id, userDto.Id);
            Assert.IsAssignableFrom<UserDto>(okResult.Value);
        }
    }
}
