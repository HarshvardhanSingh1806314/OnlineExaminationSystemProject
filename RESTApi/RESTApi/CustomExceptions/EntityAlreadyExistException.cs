using System;

namespace RESTApi.CustomExceptions
{
    public class EntityAlreadyExistException : Exception
    {
        public EntityAlreadyExistException(string message) : base(message)
        {

        }
    }
}