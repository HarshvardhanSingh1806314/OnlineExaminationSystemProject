using System;

namespace RESTApi.CustomExceptions
{
    public class NullEntityException : Exception
    {
        public NullEntityException(string message) : base(message)
        {

        }
    }
}