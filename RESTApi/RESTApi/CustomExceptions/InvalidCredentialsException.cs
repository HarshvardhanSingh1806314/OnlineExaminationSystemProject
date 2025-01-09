using System;

namespace RESTApi.CustomExceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException(string message) : base(message)
        {

        }
    }
}