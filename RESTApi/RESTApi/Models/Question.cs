using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RESTApi.Models
{
    public class Question
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Question Description cannot be empty")]
        [MaxLength(300, ErrorMessage = "Question Description Length cannot be more than 300 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Option Cannot be empty")]
        [MaxLength(200, ErrorMessage = "Option Length cannot be more than 200 characters")]
        public string Option1 { get; set; }

        [Required(ErrorMessage = "Option Cannot be Empty")]
        [MaxLength(200, ErrorMessage = "Option Length cannot be more than 200 characters")]
        public string Option2 { get; set; }

        [Required(ErrorMessage = "Option Cannot be Empty")]
        [MaxLength(200, ErrorMessage = "Option Length cannot be more than 200 characters")]
        public string Option3 { get; set; }

        [Required(ErrorMessage = "Option Cannot be Empty")]
        [MaxLength(200, ErrorMessage = "Option Length cannot be more than 200 characters")]
        public string Option4 { get; set; }

        [Required(ErrorMessage = "Answer Cannot be Empty")]
        [MaxLength(200, ErrorMessage = "Answer Length cannot be more than 200 characters")]
        public string Answer { get; set; }

        [Required(ErrorMessage = "Difficulty Level cannot be empty")]
        public string DifficultyLevelId { get; set; } 

        [ForeignKey("DifficultyLevelId")]
        public DifficultyLevel DifficultyLevel { get; set; }

        [Required(ErrorMessage = "Test cannot be empty")]
        public string TestId { get; set; }

        [ForeignKey("TestId")]
        public Test Test { get; set; }
    }
}