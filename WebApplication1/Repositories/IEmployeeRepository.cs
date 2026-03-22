using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<EmployeeResponseDto?> GetEmployeeWithRoleByIdAsync(int id);
        Task<Employee?> GetEmployeeByEmailAsync(string email);
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task<Employee?> UpdateEmployeeAsync(int id, Employee employee);
        Task<bool> SoftDeleteEmployeeAsync(int id);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role?> GetRoleByIdAsync(int id);
        Task<EmployeeRole?> GetEmployeeRoleByEmployeeIdAsync(int employeeId);
        Task AddEmployeeRoleAsync(EmployeeRole employeeRole);
        Task UpdateEmployeeRoleAsync(EmployeeRole employeeRole);
        Task SaveChangesAsync();
    }
}
