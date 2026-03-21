using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<(bool success, string message, Employee? employee)> AddEmployeeAsync(Employee employee);
        Task<(bool success, string message, Employee? employee)> UpdateEmployeeAsync(int id, Employee employee);
        Task<(bool success, string message)> DeleteEmployeeAsync(int id);
    }
}
