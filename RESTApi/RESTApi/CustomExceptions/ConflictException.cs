using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTApi.CustomExceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message)
        {

        }
    }
}