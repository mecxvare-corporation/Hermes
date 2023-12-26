using System.Diagnostics.CodeAnalysis;

namespace UserService.Domain.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException()
        {

        }

        public AlreadyExistsException(string message) : base(message)
        {

        }

        public AlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
