using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllEmployeesAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            return await _employeeRepository.GetEmployeeByIdAsync(id);
        }

        public async Task<(bool success, string message, Employee? employee)> AddEmployeeAsync(Employee employee)
        {
            try
            {
                var validationContext = new ValidationContext(employee);
                var validationResults = new List<ValidationResult>();

                if (!Validator.TryValidateObject(employee, validationContext, validationResults, true))
                {
                    var errorMessage = string.Join(", ", validationResults.Select(v => v.ErrorMessage));
                    return (false, errorMessage, null);
                }

                var addedEmployee = await _employeeRepository.AddEmployeeAsync(employee);
                return (true, "Employee added successfully", addedEmployee);
            }
            catch (Exception ex)
            {
                return (false, $"Error adding employee: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message, Employee? employee)> UpdateEmployeeAsync(int id, Employee employee)
        {
            try
            {
                if (id <= 0)
                {
                    return (false, "Invalid employee ID", null);
                }

                var validationContext = new ValidationContext(employee);
                var validationResults = new List<ValidationResult>();

                if (!Validator.TryValidateObject(employee, validationContext, validationResults, true))
                {
                    var errorMessage = string.Join(", ", validationResults.Select(v => v.ErrorMessage));
                    return (false, errorMessage, null);
                }

                var updatedEmployee = await _employeeRepository.UpdateEmployeeAsync(id, employee);
                if (updatedEmployee == null)
                {
                    return (false, "Employee not found", null);
                }

                return (true, "Employee updated successfully", updatedEmployee);
            }
            catch (Exception ex)
            {
                return (false, $"Error updating employee: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message)> DeleteEmployeeAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return (false, "Invalid employee ID");
                }

                var deleted = await _employeeRepository.DeleteEmployeeAsync(id);
                if (!deleted)
                {
                    return (false, "Employee not found");
                }

                return (true, "Employee deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting employee: {ex.Message}");
            }
        }
    }
}
