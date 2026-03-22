using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles
                .Include(r => r.EmployeeRoles)
                .ToListAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            return await _context.Roles
                .Include(r => r.EmployeeRoles)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Role> CreateRoleAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<Role?> UpdateRoleAsync(int id, Role role)
        {
            var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
            if (existingRole == null)
            {
                return null;
            }

            existingRole.RoleName = role.RoleName;

            _context.Roles.Update(existingRole);
            await _context.SaveChangesAsync();

            return existingRole;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _context.Roles
                .Include(r => r.EmployeeRoles)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
            {
                return false;
            }

            // Check if role has employees
            if (role.EmployeeRoles.Any())
            {
                return false;
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetEmployeeCountByRoleAsync(int roleId)
        {
            return await _context.EmployeeRoles
                .Where(er => er.RoleId == roleId)
                .CountAsync();
        }
    }
}
