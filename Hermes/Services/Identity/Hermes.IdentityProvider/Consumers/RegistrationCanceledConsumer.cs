using Hermes.IdentityProvider.Infrastructure.Database;
using MassTransit;
using Messages;
using Microsoft.EntityFrameworkCore;

namespace Hermes.IdentityProvider.Consumers
{
    public class RegistrationCanceledConsumer : IConsumer<RegistrationCanceled>
    {
        private readonly IdentityProviderDbContext _context;

        public RegistrationCanceledConsumer(IdentityProviderDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<RegistrationCanceled> context)
        {
            var userToDelete = await _context.Users.SingleOrDefaultAsync(u => u.SubjectId == context.Message.UserId.ToString());

            if (userToDelete != null) 
            {
                _context.Users.Remove(userToDelete);

                await _context.SaveChangesAsync();
            }
        }
    }
}
