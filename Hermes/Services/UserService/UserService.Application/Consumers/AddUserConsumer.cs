using MassTransit;
using Messages;

namespace UserService.Application.Consumers
{
    public class AddUserConsumer : IConsumer<AddUser>
    {
        public Task Consume(ConsumeContext<AddUser> context)
        {
            throw new NotImplementedException();
        }
    }
}
