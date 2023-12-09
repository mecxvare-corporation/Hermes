using Moq;
using System.Linq.Expressions;
using UserService.Application.Dtos;
using UserService.Application.Interests.Command;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Tests.Unit
{
    [Collection("MyCollection")]
    public class InterestCommandShould
    {
        private readonly IServiceProvider _serviceProvider;

        public InterestCommandShould(ServiceProviderFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }


        [Fact]
        public async Task CreateNewInterest()
        {
            // Arrange
            var newInterestDto = new CreateInterestDto("testInterest");

            // Mock IUserRepository
            var interestRepoMock = new Mock<IInterestRepository>();

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.InterestRepository).Returns(interestRepoMock.Object);

            var handler = new CreateInterestCommandHandler(uowMock.Object);
            var query = new CreateInterestCommand(newInterestDto);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            interestRepoMock.Verify(repo => repo.Create(It.IsAny<Interest>()), Times.Once);
        }

        [Fact]
        public async void DeleteExistingInterestById()
        {
            // Arrange
            string interestName = "Interest";
            var interestEntity = new Interest(interestName);

            // Mock IInterestRepository
            var interestRepoMock = new Mock<IInterestRepository>();
            interestRepoMock.Setup(repo => repo.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Interest, bool>>>())).ReturnsAsync(interestEntity);

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.InterestRepository).Returns(interestRepoMock.Object);

            var handler = new DeleteInterestCommandHandler(uowMock.Object);
            var command = new DeleteInterestCommand(interestEntity.Id);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            interestRepoMock.Verify(repo => repo.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async void NotDeleteAbsentInterestById()
        {
            // Arrange
            var mockRepository = new Mock<IInterestRepository>();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(uow => uow.InterestRepository).Returns(mockRepository.Object);

            var handler = new DeleteInterestCommandHandler(mockUnitOfWork.Object);
            var deleteInterestCommand = new DeleteInterestCommand(Guid.NewGuid());

            // Act
            async Task result() => await handler.Handle(deleteInterestCommand, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(result);
        }
    }
}
