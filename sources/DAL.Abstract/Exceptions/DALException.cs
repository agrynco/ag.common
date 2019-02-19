using System;
using System.Runtime.Serialization;

namespace DAL.Abstract.Exceptions
{
    public class DALException : Exception
    {
        public DALException()
        {
        }

        public DALException(string message) : base(message)
        {
        }

        public DALException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DALException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}