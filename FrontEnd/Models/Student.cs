using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class Student
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(8,ErrorMessage ="Password minimum length is 8 characters")]
        public string Password { get; set; }
        [Required]
        [StringLength(10,ErrorMessage ="Phone Number should contain 10 characters only")]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; } 
        [Required]
        public string GraduationYear { get; set; }
        public string City { get; set; }
        [Required]
        public string UniversityName { get; set; }
        [Required]
        public string DegreeMajor { get; set; }
    }
}