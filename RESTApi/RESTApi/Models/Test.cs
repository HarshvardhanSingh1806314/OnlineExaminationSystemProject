using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTApi.Models
{
    public class Test
    {
        [Key]
        public string TestId { get; set; }

        [Required(ErrorMessage = "Test Name cannot be empty")]
        [MaxLength(100, ErrorMessage = "Test Name Length cannot be more than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Test Description cannot be empty")]
        [MaxLength(500, ErrorMessage = "Test Description Length cannot be more than 200 characters")]
        public string Description { get; set; }

        [Required]
        public int TotalNoOfEasyQuestions { get; set; } = 0;

        [Required]
        public int TotalNoOfMediumQuestions { get; set; } = 0;

        [Required]
        public int TotalNoOfHardQuestions { get; set; } = 0;

        [Required]
        public int TotalNoOfQuestions { get; set; } = 0;

        [Required]
        public int Duration { get; set; }

        [Required]
        public bool Completed { get; set; } = false;

        [Required]
        public int AdminId { get; set; }

        [ForeignKey("AdminId")]
        [JsonIgnore]
        public Admin Admin { get; set; }
    }
}