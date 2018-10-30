using System;
using System.Runtime.Serialization;

namespace FacebookClient.Exceptions
{
    public class RemoveClientException : Exception
    {
        public RemoveClientException()
        {
        }

        public RemoveClientException(string message) : base(message)
        {
        }

        public RemoveClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RemoveClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
