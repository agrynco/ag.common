using System;

namespace Services
{
    public class BaseServiceException : ApplicationException
    {
        public BaseServiceException(string message) : base(message)
        {
        }
    }
}