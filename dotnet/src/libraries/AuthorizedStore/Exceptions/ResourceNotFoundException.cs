using System;

namespace AuthorizedStore.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException()
            : base()
        {
        }

        public ResourceNotFoundException(string message)
            : base(message)
        {
        }

        public ResourceNotFoundException(string message, Exception exception)
            : base(message, exception)
        {
        }

        public ResourceNotFoundException(string resourceType, int id)
            : this($"{resourceType} with identifier '{id}' is not existed.")
        {
        }
    }
}
