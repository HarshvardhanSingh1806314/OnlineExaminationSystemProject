using System;

namespace RESTApi.CustomExceptions
{
    public class OperationFailedException : Exception
    {
        public OperationFailedException(string message) : base(message)
        {

        }
    }
}