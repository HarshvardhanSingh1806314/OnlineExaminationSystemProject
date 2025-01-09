using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RESTApi.Models
{
    public class Report
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Student ID is required")]
        public string StudentId { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        [Required(ErrorMessage = "Test ID is required")]
        public string TestId { get; set; }

        [ForeignKey("TestId")]
        public virtual Test Test { get; set; }

        [Required(ErrorMessage = "Total Attempts Cannot be empty")]
        public int TotalAttemptsInEasyQuestions { get; set; } = 0;

        [Required(ErrorMessage = "Correct Attempts Cannot be empty")]
        public int CorrectAttempsInEasyQuestions { get; set; } = 0;

        [Required(ErrorMessage = "Total Attempts Cannot be Empty")]
        public int TotalAttemptsInMediumQuestions { get; set; } = 0;

        [Required(ErrorMessage = "Correct Attempts Cannot be Empty")]
        public int CorrectAttemptsInMediumQuestions { get; set; } = 0;

        [Required(ErrorMessage = "Total Attempts Cannot be Empty")]
        public int TotalAttemptsInHardQuestions { get; set; }

        [Required(ErrorMessage = "Correct Attempts Cannot be Empty")]
        public int CorrectAttemptsInHardQuestions { get; set; }

        [Required(ErrorMessage = "Result cannot be empty")]
        public string ResultId { get; set; }

        [ForeignKey("ResultId")]
        public virtual Result Result { get; set;}
    }
}