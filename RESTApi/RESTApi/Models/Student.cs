using RESTApi.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RESTApi.Models
{
    public class Student
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Username cannot be empty")]
        [MaxLength(100, ErrorMessage = "Username length cannot be more the 100 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email cannot be empty")]
        [MaxLength(100, ErrorMessage = "Email length cannot be more than 100 characters")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        [MinLength(8, ErrorMessage = "Password length cannot be less than 8 characters")]
        [PasswordValidation(ErrorMessage: "Password must contain atleast one uppercase, one lowercase, one digit and one special character")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone No. cannot be empty")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "DOB cannot be empty")]
        [DataType(DataType.Date)]
        public DateTime DOB {get; set;}

        [Required(ErrorMessage = "Graduation Year cannot be empty")]
        [GraduationYearValidation(ErrorMessage: "Graduation year should be greater than or equal to 1970")]
        public int GraduationYear { get; set; }

        public string City { get; set; }

        [Required(ErrorMessage = "University Name cannot be empty")]
        public string UniversityName { get; set; }

        [Required(ErrorMessage = "Degree Major cannot be empty")]
        public string DegreeMajor { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}