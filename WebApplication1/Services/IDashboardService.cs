using WebApplication1.DTOs;

namespace WebApplication1.Services
{
    public interface IDashboardService
    {
        Task<IEnumerable<EmployeesByRoleDto>> GetEmployeesByRoleAsync();
        Task<IEnumerable<EmployeesByYearDto>> GetEmployeesByYearAsync();
        Task<DashboardStatsDto> GetDashboardStatsAsync();
        Task<DashboardSummaryDto> GetDashboardSummaryAsync();
    }
}
