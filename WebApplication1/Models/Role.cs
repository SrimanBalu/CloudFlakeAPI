using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Role name must be between 2 and 50 characters")]
        public string RoleName { get; set; } = string.Empty;

        // Navigation property
        public ICollection<EmployeeRole> EmployeeRoles { get; set; } = new List<EmployeeRole>();
    }
}
