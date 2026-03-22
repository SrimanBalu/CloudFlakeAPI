using WebApplication1.DTOs;
using WebApplication1.Repositories;

namespace WebApplication1.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        /// <summary>
        /// Get active employees count grouped by role
        /// </summary>
        public async Task<IEnumerable<EmployeesByRoleDto>> GetEmployeesByRoleAsync()
        {
            try
            {
                return await _dashboardRepository.GetEmployeesByRoleAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving employees by role: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get active employees count grouped by creation year
        /// </summary>
        public async Task<IEnumerable<EmployeesByYearDto>> GetEmployeesByYearAsync()
        {
            try
            {
                return await _dashboardRepository.GetEmployeesByYearAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving employees by year: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get overall dashboard statistics
        /// </summary>
        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            try
            {
                return await _dashboardRepository.GetDashboardStatsAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving dashboard stats: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get complete dashboard summary with all statistics
        /// </summary>
        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
        {
            try
            {
                var stats = await _dashboardRepository.GetDashboardStatsAsync();
                var employeesByRole = await _dashboardRepository.GetEmployeesByRoleAsync();
                var employeesByYear = await _dashboardRepository.GetEmployeesByYearAsync();

                return new DashboardSummaryDto
                {
                    Stats = stats,
                    EmployeesByRole = employeesByRole,
                    EmployeesByYear = employeesByYear
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving dashboard summary: {ex.Message}", ex);
            }
        }
    }
}
