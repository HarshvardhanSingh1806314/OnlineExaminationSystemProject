using System.ComponentModel.DataAnnotations;

namespace RESTApi.Models
{
    public class DifficultyLevel
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Difficulty Level Name cannot be empty")]
        public string LevelName { get; set; }
    }
}