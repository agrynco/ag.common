#region Usings
using System;
using System.Runtime.Serialization;
#endregion

namespace Common
{
    public class CanNotAssignPropertyException : Exception
    {
        public CanNotAssignPropertyException()
        {
        }

        public CanNotAssignPropertyException(string message)
            : base(message)
        {
        }

        public CanNotAssignPropertyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CanNotAssignPropertyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}