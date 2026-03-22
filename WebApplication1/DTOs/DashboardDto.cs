using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class EmployeesByRoleDto
    {
        public string RoleName { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class EmployeesByYearDto
    {
        public int Year { get; set; }
        public int Count { get; set; }
    }

    public class DashboardStatsDto
    {
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int InactiveEmployees { get; set; }
        public int TotalRoles { get; set; }
    }

    public class DashboardSummaryDto
    {
        public DashboardStatsDto Stats { get; set; } = new DashboardStatsDto();
        public IEnumerable<EmployeesByRoleDto> EmployeesByRole { get; set; } = new List<EmployeesByRoleDto>();
        public IEnumerable<EmployeesByYearDto> EmployeesByYear { get; set; } = new List<EmployeesByYearDto>();
    }
}
