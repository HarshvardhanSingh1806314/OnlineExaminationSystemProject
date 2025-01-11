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
        [MinLength(6,ErrorMessage ="Employee Id minimum length is 6 characters")]
        public string Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string EmployeeEmail { get; set; }
        [Required]
        [MinLength(12,ErrorMessage ="Password minimum length should be 12 characters")]
        public string Password { get; set; }
        [Required]
        public string OrganizationName { get; set; }
        [Required]
        public string EmployeeId { get; set; }
    }
}