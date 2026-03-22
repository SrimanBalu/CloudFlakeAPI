using System.ComponentModel.DataAnnotations;
using WebApplication1.DTOs;
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

        public async Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllEmployeesAsync();
        }

        public async Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            return await _employeeRepository.GetEmployeeWithRoleByIdAsync(id);
        }

        public async Task<(bool success, string message, EmployeeResponseDto? employee)> AddEmployeeAsync(EmployeeDto employeeDto)
        {
            try
            {
                var validationContext = new ValidationContext(employeeDto);
                var validationResults = new List<ValidationResult>();

                if (!Validator.TryValidateObject(employeeDto, validationContext, validationResults, true))
                {
                    var errorMessage = string.Join(", ", validationResults.Select(v => v.ErrorMessage));
                    return (false, errorMessage, null);
                }

                // Check for duplicate email
                var existingEmployee = await _employeeRepository.GetEmployeeByEmailAsync(employeeDto.Email);
                if (existingEmployee != null)
                {
                    return (false, "Email already exists", null);
                }

                // Validate role exists
                var role = await _employeeRepository.GetRoleByIdAsync(employeeDto.RoleId);
                if (role == null)
                {
                    return (false, "Invalid role ID", null);
                }

                // Create employee with password and IsActive
                var employee = new Employee
                {
                    Name = employeeDto.Name,
                    Age = employeeDto.Age,
                    Department = employeeDto.Department,
                    Email = employeeDto.Email,
                    Phone = employeeDto.Phone,
                    Password = employeeDto.Password,
                    IsActive = employeeDto.IsActive
                };

                var addedEmployee = await _employeeRepository.AddEmployeeAsync(employee);

                // Create employee role mapping
                var employeeRole = new EmployeeRole
                {
                    EmployeeId = addedEmployee.Id,
                    RoleId = employeeDto.RoleId
                };

                await _employeeRepository.AddEmployeeRoleAsync(employeeRole);

                // Return the employee with role
                var employeeResponse = await _employeeRepository.GetEmployeeWithRoleByIdAsync(addedEmployee.Id);
                return (true, "Employee added successfully", employeeResponse);
            }
            catch (Exception ex)
            {
                return (false, $"Error adding employee: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message, EmployeeResponseDto? employee)> UpdateEmployeeAsync(int id, EmployeeUpdateDto employeeUpdateDto)
        {
            try
            {
                if (id <= 0)
                {
                    return (false, "Invalid employee ID", null);
                }

                var validationContext = new ValidationContext(employeeUpdateDto);
                var validationResults = new List<ValidationResult>();

                if (!Validator.TryValidateObject(employeeUpdateDto, validationContext, validationResults, true))
                {
                    var errorMessage = string.Join(", ", validationResults.Select(v => v.ErrorMessage));
                    return (false, errorMessage, null);
                }

                // Validate role exists
                var role = await _employeeRepository.GetRoleByIdAsync(employeeUpdateDto.RoleId);
                if (role == null)
                {
                    return (false, "Invalid role ID", null);
                }

                // Update employee
                var employee = new Employee
                {
                    Name = employeeUpdateDto.Name,
                    Age = employeeUpdateDto.Age,
                    Department = employeeUpdateDto.Department,
                    Email = employeeUpdateDto.Email,
                    Phone = employeeUpdateDto.Phone,
                    Password = employeeUpdateDto.Password ?? string.Empty, // Optional password update
                    IsActive = employeeUpdateDto.IsActive
                };

                var updatedEmployee = await _employeeRepository.UpdateEmployeeAsync(id, employee);
                if (updatedEmployee == null)
                {
                    return (false, "Employee not found", null);
                }

                // Update employee role
                var existingEmployeeRole = await _employeeRepository.GetEmployeeRoleByEmployeeIdAsync(id);
                if (existingEmployeeRole != null)
                {
                    existingEmployeeRole.RoleId = employeeUpdateDto.RoleId;
                    await _employeeRepository.UpdateEmployeeRoleAsync(existingEmployeeRole);
                }
                else
                {
                    // If no role exists, create one
                    var employeeRole = new EmployeeRole
                    {
                        EmployeeId = id,
                        RoleId = employeeUpdateDto.RoleId
                    };
                    await _employeeRepository.AddEmployeeRoleAsync(employeeRole);
                }

                // Return the employee with role
                var employeeResponse = await _employeeRepository.GetEmployeeWithRoleByIdAsync(id);
                return (true, "Employee updated successfully", employeeResponse);
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

                // Soft delete - set IsActive to false
                var deleted = await _employeeRepository.SoftDeleteEmployeeAsync(id);
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

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _employeeRepository.GetAllRolesAsync();
        }

        public async Task<(bool success, string message, EmployeeLoginDetailsDto? employee)> LoginAsync(LoginDto loginDto)
        {
            try
            {
                // Validate input
                var validationContext = new ValidationContext(loginDto);
                var validationResults = new List<ValidationResult>();

                if (!Validator.TryValidateObject(loginDto, validationContext, validationResults, true))
                {
                    var errorMessage = string.Join(", ", validationResults.Select(v => v.ErrorMessage));
                    return (false, errorMessage, null);
                }

                // Get employee by email
                var employee = await _employeeRepository.GetEmployeeByEmailAsync(loginDto.Email);
                if (employee == null)
                {
                    return (false, "Invalid email or password", null);
                }

                // Check if employee is active
                if (!employee.IsActive)
                {
                    return (false, "Employee account is inactive", null);
                }

                // Check password (plain text comparison - use BCrypt in production!)
                // TODO: In production, use BCrypt or similar for password hashing
                if (employee.Password != loginDto.Password)
                {
                    return (false, "Invalid email or password", null);
                }

                // Get employee role
                var employeeRole = await _employeeRepository.GetEmployeeRoleByEmployeeIdAsync(employee.Id);
                var roleName = string.Empty;

                if (employeeRole != null)
                {
                    var role = await _employeeRepository.GetRoleByIdAsync(employeeRole.RoleId);
                    roleName = role?.RoleName ?? string.Empty;
                }

                var loginDetails = new EmployeeLoginDetailsDto
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    RoleName = roleName
                };

                return (true, "Login successful", loginDetails);
            }
            catch (Exception ex)
            {
                return (false, $"Error during login: {ex.Message}", null);
            }
        }
    }
}
