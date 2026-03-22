using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IDashboardService dashboardService, ILogger<DashboardController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        /// <summary>
        /// Get active employees count grouped by role (Pie Chart Data)
        /// </summary>
        /// <returns>List of roles with employee counts</returns>
        [HttpGet("employees-by-role")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeesByRoleDto>>>> GetEmployeesByRole()
        {
            try
            {
                var data = await _dashboardService.GetEmployeesByRoleAsync();
                
                return Ok(new ApiResponse<IEnumerable<EmployeesByRoleDto>>
                {
                    Success = true,
                    Message = "Employees by role retrieved successfully",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employees by role");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error retrieving employees by role"
                });
            }
        }

        /// <summary>
        /// Get active employees count grouped by creation year (Line/Bar Chart Data)
        /// </summary>
        /// <returns>List of years with employee counts</returns>
        [HttpGet("employees-by-year")]
        public async Task<ActionResult<ApiResponse<IEnumerable<EmployeesByYearDto>>>> GetEmployeesByYear()
        {
            try
            {
                var data = await _dashboardService.GetEmployeesByYearAsync();
                
                return Ok(new ApiResponse<IEnumerable<EmployeesByYearDto>>
                {
                    Success = true,
                    Message = "Employees by year retrieved successfully",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employees by year");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error retrieving employees by year"
                });
            }
        }

        /// <summary>
        /// Get overall dashboard statistics (summary cards)
        /// </summary>
        /// <returns>Dashboard statistics including total, active, inactive employees and roles count</returns>
        [HttpGet("stats")]
        public async Task<ActionResult<ApiResponse<DashboardStatsDto>>> GetDashboardStats()
        {
            try
            {
                var stats = await _dashboardService.GetDashboardStatsAsync();
                
                return Ok(new ApiResponse<DashboardStatsDto>
                {
                    Success = true,
                    Message = "Dashboard statistics retrieved successfully",
                    Data = stats
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard statistics");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error retrieving dashboard statistics"
                });
            }
        }

        /// <summary>
        /// Get complete dashboard summary with all statistics
        /// </summary>
        /// <returns>Complete dashboard data including stats, employees by role, and employees by year</returns>
        [HttpGet("summary")]
        public async Task<ActionResult<ApiResponse<DashboardSummaryDto>>> GetDashboardSummary()
        {
            try
            {
                var summary = await _dashboardService.GetDashboardSummaryAsync();
                
                return Ok(new ApiResponse<DashboardSummaryDto>
                {
                    Success = true,
                    Message = "Dashboard summary retrieved successfully",
                    Data = summary
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard summary");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error retrieving dashboard summary"
                });
            }
        }
    }
}
