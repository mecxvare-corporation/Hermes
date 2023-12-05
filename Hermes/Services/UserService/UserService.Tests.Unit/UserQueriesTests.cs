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
    public class UserQueriesTests 
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        public UserQueriesTests(ServiceProviderFixture fixture) 
        {
            _serviceProvider = fixture.ServiceProvider;
            _mapper = fixture.ServiceProvider.GetRequiredService<IMapper>();
        }


        [Fact]
        public async Task GetUserQuery_ShouldReturnUserDto()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            string userName = "User";
            string userLastName = "Testadze";
            DateTime birthDay = DateTime.Now;

            var userDto = new UserDto(userId, userName, userLastName, birthDay);

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>()));

            var uofMcok = new Mock<IUnitOfWork>();
            uofMcok.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new GetUserQueryHandler(uofMcok.Object, _mapper);

            var query = new GetUserQuery(userId);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<UserDto>(result);
        }

        [Fact]
        public void GetUsersQuery_ShouldReturnUsers()
        {
            //Arrange
            //Act
            //Assert
        }
    }
}
