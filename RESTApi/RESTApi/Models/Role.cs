using System.ComponentModel.DataAnnotations;

namespace RESTApi.Models
{
    public class Role
    {
        [Key]
        public string RoleId { get; set; }

        [Required(ErrorMessage = "Role Name cannot be empty")]
        public string Name { get; set; }
    }
}