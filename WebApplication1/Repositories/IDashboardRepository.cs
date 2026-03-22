using WebApplication1.DTOs;

namespace WebApplication1.Repositories
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<EmployeesByRoleDto>> GetEmployeesByRoleAsync();
        Task<IEnumerable<EmployeesByYearDto>> GetEmployeesByYearAsync();
        Task<DashboardStatsDto> GetDashboardStatsAsync();
    }
}
