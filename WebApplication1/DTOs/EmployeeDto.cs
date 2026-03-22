using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class EmployeeDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Age is required")]
        [Range(19, 120, ErrorMessage = "Age must be greater than 18")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Department must be between 2 and 50 characters")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be exactly 10 digits")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Valid role ID is required")]
        public int RoleId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class EmployeeUpdateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Age is required")]
        [Range(19, 120, ErrorMessage = "Age must be greater than 18")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Department must be between 2 and 50 characters")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be exactly 10 digits")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string? Password { get; set; } // Optional - only update if provided

        [Required(ErrorMessage = "Role is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Valid role ID is required")]
        public int RoleId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class EmployeeResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string Password { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
    }

    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public EmployeeLoginDetailsDto? Employee { get; set; }
    }

    public class EmployeeLoginDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
}
