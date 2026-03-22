using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto?> GetRoleByIdAsync(int id);
        Task<(bool success, string message, RoleDto? role)> CreateRoleAsync(CreateRoleDto createRoleDto);
        Task<(bool success, string message, RoleDto? role)> UpdateRoleAsync(int id, UpdateRoleDto updateRoleDto);
        Task<(bool success, string message)> DeleteRoleAsync(int id);
        Task<IEnumerable<RoleWithEmployeeCountDto>> GetAllRolesWithEmployeeCountAsync();
    }
}
