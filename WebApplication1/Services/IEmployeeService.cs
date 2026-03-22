using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync();
        Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id);
        Task<(bool success, string message, EmployeeResponseDto? employee)> AddEmployeeAsync(EmployeeDto employeeDto);
        Task<(bool success, string message, EmployeeResponseDto? employee)> UpdateEmployeeAsync(int id, EmployeeUpdateDto employeeUpdateDto);
        Task<(bool success, string message)> DeleteEmployeeAsync(int id);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<(bool success, string message, EmployeeLoginDetailsDto? employee)> LoginAsync(LoginDto loginDto);
    }
}
