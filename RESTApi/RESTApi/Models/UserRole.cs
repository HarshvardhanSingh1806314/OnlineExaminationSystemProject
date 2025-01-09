using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RESTApi.Models
{
    public class UserRole
    {
        [Key]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Role Id cannot be empty")]
        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}