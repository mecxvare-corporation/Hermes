using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq.Expressions;
using UserService.Application.Dtos;
using UserService.Application.Users.Commands;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Services.ProfilePicture;
using UserService.Tests.Unit.Helpers;

namespace UserService.Tests.Unit.Commands
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
            // Arrange
            var newUserDto = new CreateUserDto("test1", "test1", DateTime.Now);

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new CreateUserCommandHandler(uowMock.Object, _serviceProvider.GetRequiredService<IMapper>());
            var query = new CreateUserCommand(newUserDto);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            userRepoMock.Verify(repo => repo.Create(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async void DeleteExistingUserById()
        {
            // Arrange
            string userName = "User";
            string userLastName = "Testadze";
            DateTime birthDay = DateTime.Now;
            var userEntity = new User(userName, userLastName, birthDay);

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(userEntity);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new DeleteUserCommandHandler(uowMock.Object);
            var query = new DeleteUserCommand(userEntity.Id);

            // Act
            await handler.Handle(query, CancellationToken.None);

            // Assert
            userRepoMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUserInterests()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var interestList = new List<Interest>()
            {
                new Interest("Interest1"),
                new Interest("Interest2")
            };

            var interestIds = interestList.Select(i => i.Id).ToList();

            var interestsQueryable = interestList.AsAsyncQueryable();

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);

            // Mock IInterestRepository
            var interestRepoMock = new Mock<IInterestRepository>();
            interestRepoMock.Setup(repo => repo.GetRowsQueryable(It.IsAny<Expression<Func<Interest, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Interest, object>>[]>())).Returns(interestsQueryable);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);
            uowMock.Setup(uow => uow.InterestRepository).Returns(interestRepoMock.Object);

            var handler = new UpdateUserInterestCommandHandler(uowMock.Object);
            var command = new UpdateUserInterestCommand(new UpdateUserInterestsDto(user.Id, interestIds));

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            foreach (var interest in interestIds)
            {
                Assert.Contains(interest, user.Interests.Select(i => i.Id));
            }

            uowMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task ThrowExceptionIfNoUserWasFoundDuringInterestAddition()
        {
            // Arrange
            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>())).ReturnsAsync((User)null);


            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new UpdateUserInterestCommandHandler(uowMock.Object);
            var command = new UpdateUserInterestCommand(new UpdateUserInterestsDto(Guid.NewGuid(), new List<Guid>()));

            // Act
            async Task result() => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(result);
        }

        [Fact]
        public async Task ThrowExceptionIfNoInterestWereRetrievedFromDb()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);

            // Mock IInterestRepository
            var interestRepoMock = new Mock<IInterestRepository>();
            interestRepoMock.Setup(repo => repo.GetRowsQueryable(It.IsAny<Expression<Func<Interest, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Interest, object>>[]>())).Returns(new List<Interest>().AsAsyncQueryable());

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);
            uowMock.Setup(uow => uow.InterestRepository).Returns(interestRepoMock.Object);

            var handler = new UpdateUserInterestCommandHandler(uowMock.Object);
            var command = new UpdateUserInterestCommand(new UpdateUserInterestsDto(user.Id, new List<Guid>()));

            // Act
            async Task Result() => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Result);
        }

        [Fact]
        public async Task UpdateItself()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new UpdateUserCommandHandler(uowMock.Object, _serviceProvider.GetRequiredService<IMapper>());
            var newUserDto = new UpdateUserDto(user.Id, "Takhma", "Gido", DateTime.Now);
            var command = new UpdateUserCommand(newUserDto);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public async Task ThrowExceptionIfNoUserWasFoundDuringUserUpdate()
        {
            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>())).ReturnsAsync((User)null);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new UpdateUserCommandHandler(uowMock.Object, _serviceProvider.GetRequiredService<IMapper>());
            var newUserDto = new UpdateUserDto(Guid.NewGuid(), "Takhma", "Gido", DateTime.Now);
            var command = new UpdateUserCommand(newUserDto);

            // Act
            async Task Result() => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Result);
        }

        [Fact]
        public async Task ThrowExceptionIfNoUserWasFoundDuringDelete()
        {
            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>())).ReturnsAsync((User)null);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new DeleteUserCommandHandler(uowMock.Object);
            var command = new DeleteUserCommand(Guid.NewGuid());

            // Act
            async Task Result() => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Result);
        }

        [Fact]
        public async Task RemoveUserInterest()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);
            var interest = new Interest("Interest1");

            user.AddInterest(interest);

            //Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);

            var interestRepositoryMock = new Mock<IInterestRepository>();
            interestRepositoryMock.Setup(repository => repository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Interest, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Interest, object>>[]>())).ReturnsAsync(interest);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);
            uowMock.Setup(uow => uow.InterestRepository).Returns(interestRepositoryMock.Object);

            var handler = new DeleteUserInterestCommandHandler(uowMock.Object);
            var command = new DeleteUserInterestCommand(new DeleteUserInterestDto(user.Id, interest.Id));

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.DoesNotContain(interest, user.Interests);
        }

        [Fact]
        public async Task AddUserProfileImage()
        {
            //Arrange
            Stream stream = new MemoryStream(new byte[5]);
            var fileName = "test.jpeg";

            var user = new User("dunda", "DUndaDUnda", DateTime.Now);

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var profileServiceMock = new Mock<IProfilePictureService>();
            var imgNameMock = $"{Guid.NewGuid()}_{fileName}";
            profileServiceMock.Setup(f => f.UploadImageAsync(It.IsAny<MemoryStream>(), It.IsAny<string>())).ReturnsAsync(imgNameMock);

            var handler = new UploadUserPictureCommandHandler(uowMock.Object, profileServiceMock.Object);
            var command = new UploadUserProfilePictureCommand(user.Id, stream, fileName);

            //Act
            var response = await handler.Handle(command, CancellationToken.None);
            var returnedString = response.Split("_");
            Guid returnedGuid;
            Guid.TryParse(returnedString[0], out returnedGuid);

            //Assert
            Assert.Equal(fileName, returnedString[1]);
            Assert.Equal(returnedGuid.ToString(), returnedString[0]);
        }

        [Fact]
        public async Task DeleteUserProfileImage()
        {
            //Arrange
            var fileName = "test";

            var user = new User("dunda", "DUndaDUnda", DateTime.Now);
            user.SetImageUri("someRandomName.jpg");

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var imageServiceMock = new Mock<IProfilePictureService>();
            imageServiceMock.Setup(f => f.DeleteImageAsync(fileName)).Returns(Task.CompletedTask);

            var handler = new DeleteUserProfileImageCommandHandler(uowMock.Object, imageServiceMock.Object);
            var command = new DeleteUserProfileImageCommand(user.Id);

            //Ac
            await handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(string.Empty, user.ProfileImage);
        }

        [Fact]
        public async Task ThrowExceptionIfUserWasNotFoundDuringImageDelete()
        {
            //Arrange
            var fileName = "test";

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync((User)null);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var imageServiceMock = new Mock<IProfilePictureService>();

            var handler = new DeleteUserProfileImageCommandHandler(uowMock.Object, imageServiceMock.Object);
            var command = new DeleteUserProfileImageCommand(Guid.NewGuid());

            //Act
            async Task Response() => await handler.Handle(command, CancellationToken.None);

            //Assert
            await Assert.ThrowsAsync<InvalidOperationException>(Response);
        }

        [Fact]
        public async Task ThrowExceptionIfUserWasNotFoundDuringImageUpload()
        {
            //Arrange
            Stream stream = new MemoryStream(new byte[5]);
            var fileName = "test.jpeg";

            var user = new User("dunda", "DUndaDUnda", DateTime.Now);

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync((User)null);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var profileServiceMock = new Mock<IProfilePictureService>();
            var imgNameMock = $"{Guid.NewGuid()}_{fileName}";
            profileServiceMock.Setup(f => f.UploadImageAsync(It.IsAny<MemoryStream>(), It.IsAny<string>())).ReturnsAsync(imgNameMock);

            var handler = new UploadUserPictureCommandHandler(uowMock.Object, profileServiceMock.Object);
            var command = new UploadUserProfilePictureCommand(user.Id, stream, fileName);

            //Act
            async Task Response() => await handler.Handle(command, CancellationToken.None);

            //Assert
            await Assert.ThrowsAsync<InvalidOperationException>(Response);
        }

        [Fact]
        public async Task ThrowExceptionIfImageWasNotUploaded()
        {
            //Arrange
            Stream stream = new MemoryStream(new byte[5]);
            var fileName = "test.jpeg";

            var user = new User("dunda", "DUndaDUnda", DateTime.Now);

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var profileServiceMock = new Mock<IProfilePictureService>();
            var imgNameMock = $"{Guid.NewGuid()}_{fileName}";
            profileServiceMock.Setup(f => f.UploadImageAsync(It.IsAny<MemoryStream>(), It.IsAny<string>())).ReturnsAsync((string)null);

            var handler = new UploadUserPictureCommandHandler(uowMock.Object, profileServiceMock.Object);
            var command = new UploadUserProfilePictureCommand(user.Id, stream, fileName);

            //Act
            async Task Response() => await handler.Handle(command, CancellationToken.None);

            //Assert
            await Assert.ThrowsAsync<InvalidOperationException>(Response);
        }

        [Fact]
        public async Task ThrowExceptionIfUserDontHaveImageDuringImageDeletion()
        {
            //Arrange
            var user = new User("dunda", "DUndaDUnda", DateTime.Now);

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var imageServiceMock = new Mock<IProfilePictureService>();

            var handler = new DeleteUserProfileImageCommandHandler(uowMock.Object, imageServiceMock.Object);
            var command = new DeleteUserProfileImageCommand(user.Id);

            //Act
            async Task Response() => await handler.Handle(command, CancellationToken.None);

            //Assert
            await Assert.ThrowsAsync<InvalidOperationException>(Response);
        }

        [Fact]
        public async Task DeleteUserImageDuringUploadIfItAlreadyHaveOne()
        {
            //Arrange
            Stream stream = new MemoryStream(new byte[5]);
            var fileName = "test.jpeg";

            var user = new User("dunda", "DUndaDUnda", DateTime.Now);
            user.SetImageUri("someImageName.jpg");

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);


            var profileServiceMock = new Mock<IProfilePictureService>();
            var imgNameMock = $"{Guid.NewGuid()}_{fileName}";
            profileServiceMock.Setup(f => f.UploadImageAsync(It.IsAny<MemoryStream>(), It.IsAny<string>())).ReturnsAsync(imgNameMock);
            profileServiceMock.Setup(d => d.DeleteImageAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            var handler = new UploadUserPictureCommandHandler(uowMock.Object, profileServiceMock.Object);
            var command = new UploadUserProfilePictureCommand(user.Id, stream, fileName);

            // Act
            async Task Result() => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Result);
        }

        [Fact]
        public async Task AddUserToFriendList()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var friend = new User("Takhma", "Gido", DateTime.Now);

            var dto = new UserFriendDto(user.Id, friend.Id);

            var command = new AddUserFriendCommand(dto);

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.userId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.friendId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(friend);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new AddUserFriendCommandHandler(uowMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(friend.Id, user.Friends.Select(x => x.Id).First());

            uowMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task RemoveFriend()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var friend = new User("Takhma", "Gido", DateTime.Now);

            var dto = new UserFriendDto(user.Id, friend.Id);

            var command = new RemoveFriendCommand(dto);

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.userId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.friendId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(friend);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            user.AddFriend(friend);

            var handler = new RemoveFriendCommandHandler(uowMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.DoesNotContain(friend, user.Friends);

            uowMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task ThrowExceptionIfNoUserWasFoundDuringRemoveUserFriend()
        {
            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>())).ReturnsAsync((User)null);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new RemoveFriendCommandHandler(uowMock.Object);
            var command = new RemoveFriendCommand(new UserFriendDto(Guid.NewGuid(), Guid.NewGuid()));

            // Act
            async Task Result() => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Result);
        }

        [Fact]
        public async Task ThrowExceptionIfNoFriendUserWasFoundDuringRemoveUserFriend()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var dto = new UserFriendDto(user.Id, Guid.NewGuid());

            var command = new RemoveFriendCommand(dto);

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.userId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.friendId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync((User)null);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new RemoveFriendCommandHandler(uowMock.Object);

            // Act
            async Task Result() => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Result);
        }

        [Fact]
        public async Task ThrowExceptionIfNoUserWasFoundDuringAddUserFriend()
        {
            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>())).ReturnsAsync((User)null);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new AddUserFriendCommandHandler(uowMock.Object);
            var command = new AddUserFriendCommand(new UserFriendDto(Guid.NewGuid(), Guid.NewGuid()));

            // Act
            async Task Result() => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Result);
        }

        [Fact]
        public async Task ThrowExceptionIfNoFriendUserWasFoundDuringAddUserFriend()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var dto = new UserFriendDto(user.Id, Guid.NewGuid());

            var command = new AddUserFriendCommand(dto);

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.userId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.friendId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync((User)null);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new AddUserFriendCommandHandler(uowMock.Object);

            // Act
            async Task Result() => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Result);
        }

        [Fact]
        public async Task ThrowExceptionIfFriendIsAlreadyAdded()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var friend = new User("Takhma", "Gido", DateTime.Now);

            var dto = new UserFriendDto(user.Id, friend.Id);

            var command = new AddUserFriendCommand(dto);

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.userId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.friendId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(friend);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            user.AddFriend(friend);

            var handler = new AddUserFriendCommandHandler(uowMock.Object);

            // Act
            async Task Result() => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<AlreadyExistsException>(Result);
        }

        [Fact]
        public async Task AddUserToFollowersList()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var follower = new User("Takhma", "Gido", DateTime.Now);

            var dto = new UserFriendDto(user.Id, follower.Id);

            var command = new AddUserFollowerCommand(dto);

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.userId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.friendId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(follower);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new AddUserFollowerCommandHandler(uowMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(follower.Id, user.Followers.Select(x => x.Id).First());

            uowMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task ThrowExceptionIfNoUserWasFoundDuringAddUserFollower()
        {
            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>())).ReturnsAsync((User)null);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new AddUserFollowerCommandHandler(uowMock.Object);
            var command = new AddUserFollowerCommand(new UserFriendDto(Guid.NewGuid(), Guid.NewGuid()));

            // Act
            async Task Result() => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Result);
        }

        [Fact]
        public async Task ThrowExceptionIfNoFollowerUserWasFoundDuringAddUserFollower()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var dto = new UserFriendDto(user.Id, Guid.NewGuid());

            var command = new AddUserFollowerCommand(dto);

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.userId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.friendId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync((User)null);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new AddUserFollowerCommandHandler(uowMock.Object);

            // Act
            async Task Result() => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Result);
        }

        [Fact]
        public async Task ThrowExceptionIfFollowerIsAlreadyAdded()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var follower = new User("Takhma", "Gido", DateTime.Now);

            var dto = new UserFriendDto(user.Id, follower.Id);

            var command = new AddUserFollowerCommand(dto);

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.userId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.friendId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(follower);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            user.AddFollower(follower);

            var handler = new AddUserFollowerCommandHandler(uowMock.Object);

            // Act
            async Task Result() => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<AlreadyExistsException>(Result);
        }

        [Fact]
        public async Task RemoveFollower()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var follower = new User("Takhma", "Gido", DateTime.Now);

            var dto = new UserFriendDto(user.Id, follower.Id);

            var command = new RemoveFollowerCommand(dto);

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.userId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.friendId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(follower);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            user.AddFriend(follower);

            var handler = new RemoveFollowerCommandHandler(uowMock.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.DoesNotContain(follower, user.Followers);

            uowMock.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task ThrowExceptionIfNoUserWasFoundDuringRemoveUserFollower()
        {
            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>())).ReturnsAsync((User)null);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new RemoveFollowerCommandHandler(uowMock.Object);
            var command = new RemoveFollowerCommand(new UserFriendDto(Guid.NewGuid(), Guid.NewGuid()));

            // Act
            async Task Result() => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Result);
        }

        [Fact]
        public async Task ThrowExceptionIfNoFriendUserWasFoundDuringRemoveUserFollower()
        {
            // Arrange
            var user = new User("Esgeso", "Namoradze", DateTime.Now);

            var dto = new UserFriendDto(user.Id, Guid.NewGuid());

            var command = new RemoveFollowerCommand(dto);

            // Mock IUserRepository
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.userId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(x => x.Id == command.dto.friendId, It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync((User)null);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var handler = new RemoveFollowerCommandHandler(uowMock.Object);

            // Act
            async Task Result() => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(Result);
        }
    }
}