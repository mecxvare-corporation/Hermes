namespace UserService.Tests.Unit
{

    [Collection("MyCollection")]
    public class TestCollectionFixture : ICollectionFixture<ServiceProviderFixture>
    {

    }

    public class ServiceProviderFixture
    {
        public IServiceProvider ServiceProvider { get; }
        public ServiceProviderFixture() 
        {
            ServiceProvider = TestStartUp.ConfigureServices();
        }
    }
}
