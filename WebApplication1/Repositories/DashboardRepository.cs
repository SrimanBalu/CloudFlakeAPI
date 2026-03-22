using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;

namespace WebApplication1.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get count of active employees grouped by role
        /// </summary>
        public async Task<IEnumerable<EmployeesByRoleDto>> GetEmployeesByRoleAsync()
        {
            return await _context.EmployeeRoles
                .Include(er => er.Role)
                .Include(er => er.Employee)
                .Where(er => er.Employee.IsActive)
                .GroupBy(er => er.Role.RoleName)
                .Select(g => new EmployeesByRoleDto
                {
                    RoleName = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToListAsync();
        }

        /// <summary>
        /// Get count of active employees grouped by creation year
        /// </summary>
        public async Task<IEnumerable<EmployeesByYearDto>> GetEmployeesByYearAsync()
        {
            return await _context.Employees
                .Where(e => e.IsActive)
                .GroupBy(e => e.CreatedAt.Year)
                .Select(g => new EmployeesByYearDto
                {
                    Year = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Year)
                .ToListAsync();
        }

        /// <summary>
        /// Get overall dashboard statistics
        /// </summary>
        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var totalEmployees = await _context.Employees.CountAsync();
            var activeEmployees = await _context.Employees.Where(e => e.IsActive).CountAsync();
            var inactiveEmployees = totalEmployees - activeEmployees;
            var totalRoles = await _context.Roles.CountAsync();

            return new DashboardStatsDto
            {
                TotalEmployees = totalEmployees,
                ActiveEmployees = activeEmployees,
                InactiveEmployees = inactiveEmployees,
                TotalRoles = totalRoles
            };
        }
    }
}
