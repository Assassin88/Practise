using System;
using System.Runtime.Serialization;

namespace FacebookClient.Exceptions
{
    public class LoginSessionException : Exception
    {
        public LoginSessionException()
        {
        }

        public LoginSessionException(string message) : base(message)
        {
        }

        public LoginSessionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LoginSessionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
