using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace FrontEnd.Models
{
    public class Admin
    {
        [Required]
        public int AdminId { get; set; }

        [Required]
        [MinLength(12,ErrorMessage ="Password minimum length should be 12 characters")]
        public string Password { get; set; }
    }
}