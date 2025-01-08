using System.ComponentModel.DataAnnotations;

namespace RESTApi.Models
{
    public class UserRole
    {
        [Key]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Role Id cannot be empty")]
        public string RoleId { get; set; }
    }
}