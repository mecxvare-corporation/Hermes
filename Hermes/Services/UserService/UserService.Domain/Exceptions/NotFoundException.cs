using System.Diagnostics.CodeAnalysis;

namespace UserService.Domain.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {

        }

        public NotFoundException(string message) : base(message)
        {

        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
