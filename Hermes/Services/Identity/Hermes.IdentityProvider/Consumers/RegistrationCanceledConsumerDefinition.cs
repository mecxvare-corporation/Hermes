using MassTransit;
using Messages;

namespace Hermes.IdentityProvider.Consumers
{
    public class RegistrationCanceledConsumerDefinition : ConsumerDefinition<RegistrationCanceledConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<RegistrationCanceledConsumer> consumerConfigurator, IRegistrationContext context)
        {
            base.ConfigureConsumer(endpointConfigurator, consumerConfigurator, context);
        }
    }
}
