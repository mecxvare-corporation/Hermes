using MassTransit;
using Messages;

namespace Hermes.IdentityProvider.Sagas
{
    public class RegisterUserSaga : MassTransitStateMachine<RegisterUserSagaData>
    {
        public State Registering { get; set; }
        public State CreatingUserProfile { get; set; }
        public State RegistrationCanceling { get; set; }

        public Event<UserRegistered> UserRegistered { get; set; }
        public Event<AddNewUser> UserProfileCreated { get; set; }
        public Event<AddNewUserFailed> AddNewUserFailed { get; set; }

        public RegisterUserSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => UserProfileCreated, e => e.CorrelateById(m => m.Message.UserId));
            Event(() => AddNewUserFailed, e => e.CorrelateById(m => m.Message.UserId));
            Event(() => UserRegistered, e => e.CorrelateById(m => m.Message.UserId));

            Initially(
                When(UserRegistered)
                    .Then(context =>
                    {
                        context.Saga.UserId = context.Message.UserId;
                        context.Saga.UserRegistered = true;
                    })
                    .TransitionTo(Registering)
                    .Publish(context => new AddNewUser
                    {
                        UserId = context.Message.UserId,
                        DateOfBirth = context.Message.DateOfBirth,
                        FirstName = context.Message.FirstName,
                        LastName = context.Message.LastName,
                    }));

            During(Registering,
                When(UserProfileCreated)
                    .Then(context =>
                    {
                        context.Saga.UserProfileCreated = true;
                    })
                    .TransitionTo(CreatingUserProfile)
                    );

            During(CreatingUserProfile,
                When(AddNewUserFailed)
                    .Then(context =>
                    {
                        context.Saga.UserId = context.Message.UserId;
                        context.Saga.UserProfileCreated = false;
                        context.Saga.UserRegistered = false;
                    })
                    .TransitionTo(RegistrationCanceling)
                    .Publish(context => new RegistrationCanceled { UserId = context.Message.UserId, CreatedAt = DateTime.UtcNow }));
        }
    }
}
