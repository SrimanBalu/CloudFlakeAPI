using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role?> GetRoleByIdAsync(int id);
        Task<Role> CreateRoleAsync(Role role);
        Task<Role?> UpdateRoleAsync(int id, Role role);
        Task<bool> DeleteRoleAsync(int id);
        Task<int> GetEmployeeCountByRoleAsync(int roleId);
    }
}
