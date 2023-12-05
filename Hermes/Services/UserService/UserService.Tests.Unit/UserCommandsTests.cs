namespace UserService.Tests.Unit
{
    [Collection("MyCollection")]
    public class UserCommandsTests
    {
        private readonly IServiceProvider _serviceProvider;
        public UserCommandsTests(ServiceProviderFixture fixture) 
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        [Fact]
        public async Task CreateUserCommand_ShouldSucceed()
        {
            ////Arrange
            //var mockUow = new Mock<IUnitOfWork>();
            //var createUserCommand = new CreateUserCommand(new CreateUserDto("TestName", "TestLastName", new DateTime()));
            //var createUserCommandHandler = new CreateUserCommandHandler(mockUow.Object, mockMapper.Object);

            //mockMapper.Setup(mapper => mapper.Map<User>(It.IsAny<CreateUserDto>())).Returns(new User("TestName", "TestLastName", new DateTime()));
            ////Act
            //await createUserCommandHandler.Handle(createUserCommand, CancellationToken.None);

            ////Assert
            //mockMediator.Verify(mediator=>mediator.Send(It.IsAny<CreateUserCommand>(), CancellationToken.None), Times.Once);
            //mockMapper.Verify(mapper => mapper.Map<User>(It.IsAny<CreateUserDto>()), Times.Once);

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
