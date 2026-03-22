namespace WebApplication1.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
    }

    public class CreateRoleDto
    {
        public string RoleName { get; set; } = string.Empty;
    }

    public class UpdateRoleDto
    {
        public string RoleName { get; set; } = string.Empty;
    }

    public class RoleDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }

    public class RoleWithEmployeeCountDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public int EmployeeCount { get; set; }
    }
}
