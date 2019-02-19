#region Usings
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
#endregion

namespace Common
{
    public class ThereIsNoPropertyException : Exception
    {
        public ThereIsNoPropertyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ThereIsNoPropertyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ThereIsNoPropertyException(string message)
            : base(message)
        {
        }

        public ThereIsNoPropertyException()
        {
        }

        public ThereIsNoPropertyException(string propertyName, object obj)
            : this(string.Format("Type {0} dose not containe property '{1}'", obj.GetType(), propertyName))
        {
        }

        public ThereIsNoPropertyException(IEnumerable<string> propertyNames, object obj)
            : this(string.Format("Type {0} dose not contain properties '{1}'", obj.GetType(),
                string.Join(", ", propertyNames)))
        {
        }
    }
}