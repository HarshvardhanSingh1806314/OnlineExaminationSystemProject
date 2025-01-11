using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class StudentLogin
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(8,ErrorMessage ="Minimum length of password is 8 characters")]
        public string Password { get; set; }
    }
}