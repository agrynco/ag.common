using System;
using System.Runtime.Serialization;

namespace DAL.Abstract.Exceptions
{
    public class ThereIsNoEntityException : DALException
    {
        public ThereIsNoEntityException()
        {
        }

        public ThereIsNoEntityException(string message) : base(message)
        {
        }

        public ThereIsNoEntityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ThereIsNoEntityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}