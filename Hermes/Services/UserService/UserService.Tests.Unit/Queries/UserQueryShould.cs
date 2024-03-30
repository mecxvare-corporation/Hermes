using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq.Expressions;
using UserService.Application.Dtos;
using UserService.Application.Users.Queries;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Services.ProfilePicture;

namespace UserService.Tests.Unit.Queries
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
            Guid userId = Guid.NewGuid();
            DateTime birthDay = DateTime.Now;
            var userEntity = new User(userId, userName, userLastName, birthDay);

            var userDto = new UserDto(userId, userName, userLastName, birthDay, "image.jpg");

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(userEntity);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            // Mock IProfileService
            var profileServiceMock = new Mock<IProfilePictureService>();
            profileServiceMock.Setup(img => img.GetImageUrl(userDto.ProfileImage)).ReturnsAsync("guid+name");

            var handler = new GetUserQueryHandler(uowMock.Object, _serviceProvider.GetRequiredService<IMapper>(), profileServiceMock.Object);

            var query = new GetUserQuery(userId);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<UserDto>(result);
        }

        [Fact]
        public async Task ThrowExceptionIfNoUserWasFound()
        {
            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync((User)null);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            // Mock IProfileService
            var profileServiceMock = new Mock<IProfilePictureService>();

            var handler = new GetUserQueryHandler(uowMock.Object, _serviceProvider.GetRequiredService<IMapper>(), profileServiceMock.Object);

            var query = new GetUserQuery(Guid.NewGuid());

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task GetAllUsers()
        {
            //Arrange
            List<User> users = new List<User>()
            {
                new User(Guid.NewGuid(), "test1", "test1", DateTime.Now),
                new User(Guid.NewGuid(), "test2", "test2", DateTime.Now),
                new User(Guid.NewGuid(), "test3", "test3", DateTime.Now),
            };

            List<UserDto> usersDto = new List<UserDto>()
            {
                new UserDto(users[0].Id, users[0].FirstName, users[0].LastName, users[0].DateOfBirth, "image.jpg"),
                new UserDto(users[1].Id, users[1].FirstName, users[1].LastName, users[1].DateOfBirth, "image.jpg"),
                new UserDto(users[2].Id, users[2].FirstName, users[2].LastName, users[2].DateOfBirth, "image.jpg"),
            };

            //Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            //Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            //Mock IProfileService
            var profileServiceMock = new Mock<IProfilePictureService>();
            //profileServiceMock.Setup(img => img.GetImageUrl(userDto.ProfileImage)).ReturnsAsync("guid+name");

            var handler = new GetUsersQueryHandler(uowMock.Object, _serviceProvider.GetRequiredService<IMapper>(), profileServiceMock.Object);
            var query = new GetUsersQuery();

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<UserDto>>(result);
            Assert.Equal(users.Count, result.ToList().Count);
        }

        [Fact]
        public async Task GetAllUserInterests()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "Esgeso", "Namoradze", DateTime.Now);
            user.AddInterest(new Interest("Interest1"));
            user.AddInterest(new Interest("Interest2"));

            //Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);

            //Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new GetUserInterestsQueryHandler(uowMock.Object, _serviceProvider.GetRequiredService<IMapper>());
            var query = new GetUserInterestsQuery(user.Id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<UserInterestsDto>(result);
            Assert.Equal(user.Interests.Count, result.Interests.Count);
        }

        [Fact]
        public async Task ThrowExceptionIfUserWasNotFound()
        {
            // Arrange

            //Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>())).ReturnsAsync((User)null);

            //Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new GetUserInterestsQueryHandler(uowMock.Object, _serviceProvider.GetRequiredService<IMapper>());
            var query = new GetUserInterestsQuery(Guid.NewGuid());

            // Act
            async Task result() => await handler.Handle(query, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(result);
        }

        [Fact]
        public async Task ReturnEmptyListIfNoUserWasFound()
        {
            //Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<User>());

            //Mock ProfilePictureService
            var profileServiceMock = new Mock<IProfilePictureService>();
            profileServiceMock.Setup(f => f.GetImageUrl(It.IsAny<string>())).ReturnsAsync(string.Empty);

            //Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new GetUsersQueryHandler(uowMock.Object, _serviceProvider.GetRequiredService<IMapper>(), profileServiceMock.Object);
            var query = new GetUsersQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<UserDto>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ReturnAllUserFriends()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "Esgeso", "Namoradze", DateTime.Now);

            user.AddFriend(new User(Guid.NewGuid(), "Takhma", "Gido", DateTime.Now));

            //Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);

            //Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new GetUserFriendsQueryHandler(uowMock.Object, _serviceProvider.GetRequiredService<IMapper>());
            var query = new GetUserFriendsQuery(user.Id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<GetUserFriendsDto>(result);
            Assert.Equal(user.Friends.Count, result.friends.Count);
        }

        [Fact]
        public async Task ThrowExceptionIfNoUserWasFoundDuringGetUserFriends()
        {
            //Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync((User)null);

            //Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new GetUserFriendsQueryHandler(uowMock.Object, _serviceProvider.GetRequiredService<IMapper>());
            var query = new GetUserFriendsQuery(Guid.NewGuid());

            // Act
            async Task Result() => await handler.Handle(query, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Result);
        }

        [Fact]
        public async Task ReturnAllUserFollowers()
        {
            // Arrange
            var user = new User(Guid.NewGuid(), "Esgeso", "Namoradze", DateTime.Now);

            user.AddFollower(new User(Guid.NewGuid(), "Takhma", "Gido", DateTime.Now));

            //Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);

            //Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new GetUserFollowersQueryHandler(uowMock.Object, _serviceProvider.GetRequiredService<IMapper>());
            var query = new GetUserFollowersQuery(user.Id);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<GetUserFriendsDto>(result);
            Assert.Equal(user.Followers.Count, result.friends.Count);
        }

        [Fact]
        public async Task ThrowExceptionIfNoUserWasFoundDuringGetUserFollowers()
        {
            //Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync((User)null);

            //Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new GetUserFollowersQueryHandler(uowMock.Object, _serviceProvider.GetRequiredService<IMapper>());
            var query = new GetUserFollowersQuery(Guid.NewGuid());

            // Act
            async Task Result() => await handler.Handle(query, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Result);
        }
    }
}
