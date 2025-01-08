using RESTApi.CustomValidations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RESTApi.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(100, ErrorMessage = "Username Length cannot be more than 100")]
        [MinLength(15, ErrorMessage ="Username Length cannot be less than 15")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [MaxLength(100, ErrorMessage = "Email length cannot be more than 100 characters")]
        [EmailAddress(ErrorMessage = "Invalid Email Address Format")]
        public string EmployeeEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password Length cannot be less than 8")]
        [PasswordValidation(ErrorMessage: "Password must contain atleast one lowercase letter, one uppercase letter, one digit and one special character")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Organization Name is required")]
        public string OrganizationName { get; set; }

        [Required(ErrorMessage = "EmployeeID is Required")]
        public int EmployeeId { get; set; }

        // Navigation Property For Tests
        public ICollection<Test> Tests { get; set; }
    }
}