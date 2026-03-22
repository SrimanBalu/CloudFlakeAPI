using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                
                .Include(e => e.EmployeeRoles)
                    .ThenInclude(er => er.Role)
                .Select(e => new EmployeeResponseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Age = e.Age,
                    Department = e.Department,
                    Email = e.Email,
                    Phone = e.Phone,
                    Password = e.Password,
                    RoleName = e.EmployeeRoles.FirstOrDefault() != null 
                        ? e.EmployeeRoles.FirstOrDefault()!.Role.RoleName 
                        : string.Empty,
                    RoleId = e.EmployeeRoles.FirstOrDefault()!.Role.Id,
                    IsActive = e.IsActive
                })
                .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Id == id && e.IsActive);
        }

        public async Task<EmployeeResponseDto?> GetEmployeeWithRoleByIdAsync(int id)
        {
            return await _context.Employees
                .Where(e => e.Id == id && e.IsActive)
                .Include(e => e.EmployeeRoles)
                    .ThenInclude(er => er.Role)
                .Select(e => new EmployeeResponseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Age = e.Age,
                    Department = e.Department,
                    Email = e.Email,
                    Phone = e.Phone,
                    Password = e.Password,
                    RoleName = e.EmployeeRoles.FirstOrDefault() != null
                        ? e.EmployeeRoles.FirstOrDefault()!.Role.RoleName
                        : string.Empty,
                    RoleId = e.EmployeeRoles.FirstOrDefault()!.Role.Id,
                    IsActive = e.IsActive
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Employee?> GetEmployeeByEmailAsync(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            employee.CreatedAt = DateTime.UtcNow;
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee?> UpdateEmployeeAsync(int id, Employee employee)
        {
            var existingEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (existingEmployee == null)
            {
                return null;
            }

            existingEmployee.Name = employee.Name;
            existingEmployee.Age = employee.Age;
            existingEmployee.Department = employee.Department;
            existingEmployee.Email = employee.Email;
            existingEmployee.Phone = employee.Phone;
            
            // Only update password if provided
            if (!string.IsNullOrEmpty(employee.Password))
            {
                existingEmployee.Password = employee.Password;
            }
            
            existingEmployee.IsActive = employee.IsActive;
            existingEmployee.UpdatedAt = DateTime.UtcNow;

            _context.Employees.Update(existingEmployee);
            await _context.SaveChangesAsync();

            return existingEmployee;
        }

        public async Task<bool> SoftDeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            
            if (employee == null)
            {
                return false;
            }

            employee.IsActive = false;
            employee.UpdatedAt = DateTime.UtcNow;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<EmployeeRole?> GetEmployeeRoleByEmployeeIdAsync(int employeeId)
        {
            return await _context.EmployeeRoles
                .FirstOrDefaultAsync(er => er.EmployeeId == employeeId);
        }

        public async Task AddEmployeeRoleAsync(EmployeeRole employeeRole)
        {
            _context.EmployeeRoles.Add(employeeRole);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeRoleAsync(EmployeeRole employeeRole)
        {
            _context.EmployeeRoles.Update(employeeRole);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
