using AutoMapper;
using Azure.Core;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq.Expressions;
using UserService.Application.Dtos;
using UserService.Application.Users.Commands;
using UserService.Domain.Entities;
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
            async Task result() => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(result);
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
            async Task result() => await handler.Handle(command, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(result);
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

        //[Fact]
        //public async Task AddProfileImage()
        //{
        //    //Arrange
        //    Stream stream = new MemoryStream(new byte[5]);
        //    var fileName = "test.jpeg";

        //    var user = new User("dunda", "DUndaDUnda", DateTime.Now);

        //    var userRepoMock = new Mock<IUserRepository>();
        //    userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);

        //    var uowMock = new Mock<IUnitOfWork>();
        //    uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

        //    var profileServiceMock = new Mock<IProfilePictureService>();
        //    var imgNameMock = $"{Guid.NewGuid()}_{fileName}";
        //    profileServiceMock.Setup(f => f.UploadImageAsync(stream, imgNameMock)).ReturnsAsync(imgNameMock);

        //    var handler = new UploadUserPictureCommandHandler(uowMock.Object, profileServiceMock.Object);
        //    var command = new UploadUserProfilePictureCommand(user.Id, stream, fileName);

        //    //Act
        //    var response = await handler.Handle(command, CancellationToken.None);
        //    var returnedString = response.Split("_");
        //    Guid returnedGuid;
        //    Guid.TryParse(returnedString[0], out returnedGuid);

        //    //Assert
        //    Assert.Equal(fileName, returnedString[1]);
        //    Assert.Equal(returnedGuid.ToString(), returnedString[0]);
        //}


        [Fact]
        public async Task DeleteUserImage()
        {
            //Arrange
            var fileName = "test";

            var user = new User("dunda", "DUndaDUnda", DateTime.Now);

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<User, object>>[]>())).ReturnsAsync(user);

            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.UserRepository).Returns(userRepoMock.Object);

            var imageServiceMock = new Mock<IProfilePictureService>();
            imageServiceMock.Setup(f=> f.DeleteImageAsync(fileName)).Returns(Task.CompletedTask);

            var handler = new DeleteUserProfileImageCommandHandler(uowMock.Object, imageServiceMock.Object);
            var command = new DeleteUserProfileImageCommand(user.Id);

            //Act
            await handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal(string.Empty, user.ProfileImage);
        }

    }
}
