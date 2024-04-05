using System.Runtime.Serialization;

namespace ExpenseProject.Core.CustomExceptions
{
    public class UsernameAlreadyExists : Exception
    {
        public UsernameAlreadyExists()
        {
        }

        public UsernameAlreadyExists(string? message) : base(message)
        {
        }

        public UsernameAlreadyExists(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UsernameAlreadyExists(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
