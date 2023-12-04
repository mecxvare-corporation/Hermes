using AutoMapper;
using MediatR;
using Moq;
using UserService.Application.Dtos;
using UserService.Application.Users.Commands;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Tests.Unit
{
    public class UserCommandsTests
    {
        [Fact]
        public async Task CreateUserCommand_ShouldSucceed()
        {
            //Arrange
            var mockMediator = new Mock<IMediator>();
            var mockUow = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var createUserCommand = new CreateUserCommand(new CreateUserDto("TestName", "TestLastName", new DateTime()));
            var createUserCommandHandler = new CreateUserCommandHandler(mockUow.Object, mockMapper.Object);

            mockMapper.Setup(mapper => mapper.Map<User>(It.IsAny<CreateUserDto>())).Returns(new User("TestName", "TestLastName", new DateTime()));
            //Act
            await createUserCommandHandler.Handle(createUserCommand, CancellationToken.None);

            //Assert
            mockMediator.Verify(mediator=>mediator.Send(It.IsAny<CreateUserCommand>(), CancellationToken.None), Times.Once);
            mockMapper.Verify(mapper => mapper.Map<User>(It.IsAny<CreateUserDto>()), Times.Once);

        }

        [Fact]
        public void UpdateUserCommand_ShouldSucceed()
        {
            //Arrange
            //Act
            //Assert
        }

        [Fact]
        public void DeleteUserCommand_ShouldSucceed() 
        {
            //Arrange
            //Act
            //Assert
        }
    }
}
