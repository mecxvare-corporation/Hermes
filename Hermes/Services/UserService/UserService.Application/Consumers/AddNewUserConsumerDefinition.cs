using MassTransit;

namespace UserService.Application.Consumers
{
    public class AddNewUserConsumerDefinition: ConsumerDefinition<AddNewUserConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<AddNewUserConsumer> consumerConfigurator, IRegistrationContext context)
        {
            base.ConfigureConsumer(endpointConfigurator, consumerConfigurator, context);
        }
    }
}
