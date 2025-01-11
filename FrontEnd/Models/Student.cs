using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Models
{
    public class Student
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DOB { get; set; } 
        public int GraduationYear { get; set; }
        public string City { get; set; }
        public string UniversityName { get; set; }
        public string DegreeMajor { get; set; }
    }
}