using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using UserService.Application.Dtos;
using UserService.Application.Users.Commands;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Tests.Unit
{
    [Collection("MyCollection")]
    public class UserCommandShould
    {
        private readonly IServiceProvider _serviceProvider;
        public UserCommandShould(ServiceProviderFixture fixture) 
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        [Fact]
        public async Task CreateNewUser()
        {
            //Arrange
            var newUserDto = new CreateUserDto("test1", "test1", DateTime.Now);

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();

            // Mock IUnitOfWork
            var uofMcok = new Mock<IUnitOfWork>();
            uofMcok.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new CreateUserCommandHandler(uofMcok.Object, _serviceProvider.GetRequiredService<IMapper>());
            var query = new CreateUserCommand(newUserDto);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            userRepoMock.Verify(repo=>repo.Create(It.IsAny<User>()), Times.Once);
        }


        [Fact]
        public async void DeleteExistingUserById() 
        {
            //Arrange

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();

            // Mock IUnitOfWork
            var uofMcok = new Mock<IUnitOfWork>();
            uofMcok.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new DeleteUserCommandHandler(uofMcok.Object);
            var query = new DeleteUserCommand(Guid.NewGuid());

            //Act
            await handler.Handle(query, CancellationToken.None);

            //Assert
            userRepoMock.Verify(repo=>repo.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
