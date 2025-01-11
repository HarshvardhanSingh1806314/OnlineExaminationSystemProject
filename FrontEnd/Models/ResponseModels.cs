using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class ResponseModels
    {
        public struct LoginModel
        {
            public string AccessToken { get; set; }
        }
    }
}