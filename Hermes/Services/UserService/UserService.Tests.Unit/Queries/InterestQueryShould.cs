using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using UserService.Application.Dtos;
using UserService.Application.Interests.Query;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;
using UserService.Domain.Interfaces;

namespace UserService.Tests.Unit.Queries
{
    [Collection("MyCollection")]
    public class InterestQueryShould
    {
        private readonly IServiceProvider _serviceProvider;

        public InterestQueryShould(ServiceProviderFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
        }

        [Fact]
        public async Task RetrieveAllInterestsAsync()
        {
            // Arrange
            List<Interest> interests = new()
            {
                new Interest("interest1"),
                new Interest("Interest2")
            };

            List<InterestDto> interestsDto = new()
            {
                new InterestDto(interests[0].Id, interests[0].Name),
                new InterestDto(interests[1].Id, interests[1].Name)
            };

            // Mock IUserRepository
            var interestRepoMock = new Mock<IInterestRepository>();
            interestRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(interests);

            //Mock IUnitOfWork
            var uofMock = new Mock<IUnitOfWork>();
            uofMock.Setup(uow => uow.InterestRepository).Returns(interestRepoMock.Object);

            // Act
            var handler = new GetAllInterestsQueryHandler(uofMock.Object, _serviceProvider.GetRequiredService<IMapper>());
            var query = new GetAllInterestsQuery();

            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<InterestDto>>(result);
            Assert.Equal(interests.Count, result.ToList().Count);
        }

        [Fact]
        public void ThrowExceptionIfNoInterestIsPresent()
        {
            // Mock IInterestRepository
            var interestRepoMock = new Mock<IInterestRepository>();
            interestRepoMock.Setup(repo => repo.GetAllAsync());

            // Mock IUnitOfWork
            var uowMock = new Mock<IUnitOfWork>();
            uowMock.Setup(uow => uow.InterestRepository).Returns(interestRepoMock.Object);

            var handler = new GetAllInterestsQueryHandler(uowMock.Object, _serviceProvider.GetRequiredService<IMapper>());
            var query = new GetAllInterestsQuery();

            // Assert
            Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
