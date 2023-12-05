using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq.Expressions;
using UserService.Application.Dtos;
using UserService.Application.Users.Queries;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Tests.Unit
{
    [Collection("MyCollection")]
    public class UserQueryShould 
    {
        private readonly IServiceProvider _serviceProvider;
 
        public UserQueryShould(ServiceProviderFixture fixture) 
        {
            _serviceProvider = fixture.ServiceProvider;
        }


        [Fact]
        public async Task GetSingleUserById()
        {
            //Arrange
            string userName = "User";
            string userLastName = "Testadze";
            DateTime birthDay = DateTime.Now;
            var userEntity = new User(userName, userLastName, birthDay);
            Guid userId = userEntity.Id;

            var userDto = new UserDto(userId, userName, userLastName, birthDay);

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(userEntity);

            // Mock IUnitOfWork
            var uofMcok = new Mock<IUnitOfWork>();
            uofMcok.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new GetUserQueryHandler(uofMcok.Object, _serviceProvider.GetRequiredService<IMapper>());

            var query = new GetUserQuery(userId);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<UserDto>(result);
        }

        [Fact]
        public async Task GetAllUsers()
        {
            //Arrange
            List<User> users = new List<User>()
            {
                new User("test1", "test1", DateTime.Now),
                new User("test2", "test2", DateTime.Now),
                new User("test3", "test3", DateTime.Now),
            };

            List<UserDto> usersDto = new List<UserDto>()
            {
                new UserDto(users[0].Id, users[0].FirstName, users[0].LastName, users[0].DateOfBirth),
                new UserDto(users[1].Id, users[1].FirstName, users[1].LastName, users[1].DateOfBirth),
                new UserDto(users[2].Id, users[2].FirstName, users[2].LastName, users[2].DateOfBirth),
            };

            //Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            //Mock IUnitOfWork
            var uofMock = new Mock<IUnitOfWork>();
            uofMock.Setup(uow=>uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new GetUsersQueryHandler(uofMock.Object, _serviceProvider.GetRequiredService<IMapper>());
            var query = new GetUsersQuery();

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<UserDto>>(result);
            Assert.Equal(users.Count, result.ToList().Count);
        }
    }
}
