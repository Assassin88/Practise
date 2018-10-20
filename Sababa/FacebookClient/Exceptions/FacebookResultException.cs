using System;
using System.Runtime.Serialization;

namespace FacebookClient.Exceptions
{
    public class FacebookResultException : Exception
    {
        public FacebookResultException()
        {
        }

        public FacebookResultException(string message) : base(message)
        {
        }

        public FacebookResultException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FacebookResultException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
