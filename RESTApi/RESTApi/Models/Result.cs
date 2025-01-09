using System.ComponentModel.DataAnnotations;

namespace RESTApi.Models
{
    public class Result
    {
        [Key]
        public string ResultId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}