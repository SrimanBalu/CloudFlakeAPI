using System.ComponentModel.DataAnnotations;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            return roles.Select(r => new RoleDto
            {
                Id = r.Id,
                RoleName = r.RoleName
            });
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            var role = await _roleRepository.GetRoleByIdAsync(id);
            if (role == null)
            {
                return null;
            }

            return new RoleDto
            {
                Id = role.Id,
                RoleName = role.RoleName
            };
        }

        public async Task<(bool success, string message, RoleDto? role)> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            try
            {
                var validationContext = new ValidationContext(createRoleDto);
                var validationResults = new List<ValidationResult>();

                if (!Validator.TryValidateObject(createRoleDto, validationContext, validationResults, true))
                {
                    var errorMessage = string.Join(", ", validationResults.Select(v => v.ErrorMessage));
                    return (false, errorMessage, null);
                }

                // Check for duplicate role name
                var existingRoles = await _roleRepository.GetAllRolesAsync();
                if (existingRoles.Any(r => r.RoleName.Equals(createRoleDto.RoleName, StringComparison.OrdinalIgnoreCase)))
                {
                    return (false, "Role name already exists", null);
                }

                var role = new Role
                {
                    RoleName = createRoleDto.RoleName
                };

                var createdRole = await _roleRepository.CreateRoleAsync(role);

                return (true, "Role created successfully", new RoleDto
                {
                    Id = createdRole.Id,
                    RoleName = createdRole.RoleName
                });
            }
            catch (Exception ex)
            {
                return (false, $"Error creating role: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message, RoleDto? role)> UpdateRoleAsync(int id, UpdateRoleDto updateRoleDto)
        {
            try
            {
                if (id <= 0)
                {
                    return (false, "Invalid role ID", null);
                }

                var validationContext = new ValidationContext(updateRoleDto);
                var validationResults = new List<ValidationResult>();

                if (!Validator.TryValidateObject(updateRoleDto, validationContext, validationResults, true))
                {
                    var errorMessage = string.Join(", ", validationResults.Select(v => v.ErrorMessage));
                    return (false, errorMessage, null);
                }

                // Check for duplicate role name (excluding current role)
                var existingRoles = await _roleRepository.GetAllRolesAsync();
                if (existingRoles.Any(r => r.Id != id && r.RoleName.Equals(updateRoleDto.RoleName, StringComparison.OrdinalIgnoreCase)))
                {
                    return (false, "Role name already exists", null);
                }

                var role = new Role
                {
                    RoleName = updateRoleDto.RoleName
                };

                var updatedRole = await _roleRepository.UpdateRoleAsync(id, role);
                if (updatedRole == null)
                {
                    return (false, "Role not found", null);
                }

                return (true, "Role updated successfully", new RoleDto
                {
                    Id = updatedRole.Id,
                    RoleName = updatedRole.RoleName
                });
            }
            catch (Exception ex)
            {
                return (false, $"Error updating role: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message)> DeleteRoleAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return (false, "Invalid role ID");
                }

                var deleted = await _roleRepository.DeleteRoleAsync(id);
                if (!deleted)
                {
                    return (false, "Role not found or has associated employees");
                }

                return (true, "Role deleted successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting role: {ex.Message}");
            }
        }

        public async Task<IEnumerable<RoleWithEmployeeCountDto>> GetAllRolesWithEmployeeCountAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            var roleList = new List<RoleWithEmployeeCountDto>();

            foreach (var role in roles)
            {
                var employeeCount = await _roleRepository.GetEmployeeCountByRoleAsync(role.Id);
                roleList.Add(new RoleWithEmployeeCountDto
                {
                    Id = role.Id,
                    RoleName = role.RoleName,
                    EmployeeCount = employeeCount
                });
            }

            return roleList;
        }
    }
}
