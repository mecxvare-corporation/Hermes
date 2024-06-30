using MassTransit;

namespace Hermes.IdentityProvider.Sagas
{
    public class RegisterUserSagaData : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public string CurrentState { get; set; }

        public Guid UserId { get; set; }

        public bool UserRegistered { get; set; }

        public bool UserProfileCreated { get; set; }

    }
}
