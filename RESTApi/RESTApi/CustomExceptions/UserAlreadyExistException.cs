using System;

namespace RESTApi.CustomExceptions
{
    public class UserAlreadyExistException : Exception
    {
        public UserAlreadyExistException(string message) : base(message)
        {

        }
    }
}