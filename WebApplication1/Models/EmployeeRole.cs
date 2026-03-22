using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class EmployeeRole
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int RoleId { get; set; }

        // Navigation properties
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; } = null!;

        [ForeignKey("RoleId")]
        public Role Role { get; set; } = null!;
    }
}
