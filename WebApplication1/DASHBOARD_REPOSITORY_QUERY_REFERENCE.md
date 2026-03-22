# Dashboard Repository - LINQ Query Reference

## Overview
This document explains the LINQ queries used in `DashboardRepository.cs` for fetching dashboard statistics.

---

## Query 1: GetEmployeesByRoleAsync()

### LINQ Code
```csharp
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
```

### SQL Equivalent
```sql
SELECT 
    r.RoleName,
    COUNT(e.Id) as Count
FROM EmployeeRoles er
JOIN Roles r ON er.RoleId = r.Id
JOIN Employees e ON er.EmployeeId = e.Id
WHERE e.IsActive = 1
GROUP BY r.RoleName
ORDER BY Count DESC
```

### Step-by-Step Breakdown

**Step 1: Starting Point**
```csharp
_context.EmployeeRoles
```
Start with EmployeeRoles table (the junction table)

**Step 2: Load Related Data**
```csharp
.Include(er => er.Role)
.Include(er => er.Employee)
```
- Include Role information (to get RoleName)
- Include Employee information (to check IsActive)

**Step 3: Filter Active Employees**
```csharp
.Where(er => er.Employee.IsActive)
```
Only include employees where IsActive = true
- Excludes soft-deleted (inactive) employees

**Step 4: Group by Role**
```csharp
.GroupBy(er => er.Role.RoleName)
```
Group all EmployeeRoles by RoleName
- All Developers together
- All HR together
- All Admins together

**Step 5: Create Result Object**
```csharp
.Select(g => new EmployeesByRoleDto
{
    RoleName = g.Key,      // The role name (from GroupBy)
    Count = g.Count()      // Number of employees in group
})
```
Project grouped data into DTO

**Step 6: Sort Results**
```csharp
.OrderByDescending(x => x.Count)
```
Sort by count (highest first) for visual priority

**Step 7: Execute Query**
```csharp
.ToListAsync()
```
Execute on database and return as list

### Example Result
| RoleName | Count |
|----------|-------|
| Developer | 15 |
| HR | 10 |
| Admin | 5 |

---

## Query 2: GetEmployeesByYearAsync()

### LINQ Code
```csharp
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
```

### SQL Equivalent
```sql
SELECT 
    YEAR(CreatedAt) as Year,
    COUNT(Id) as Count
FROM Employees
WHERE IsActive = 1
GROUP BY YEAR(CreatedAt)
ORDER BY Year ASC
```

### Step-by-Step Breakdown

**Step 1: Starting Point**
```csharp
_context.Employees
```
Start with Employees table

**Step 2: Filter Active Only**
```csharp
.Where(e => e.IsActive)
```
Only include active employees
- Excludes soft-deleted records

**Step 3: Extract Year from CreatedAt**
```csharp
.GroupBy(e => e.CreatedAt.Year)
```
Group by the year portion of CreatedAt
- 2022-01-15 ? 2022
- 2023-06-20 ? 2023
- 2024-03-10 ? 2024

**Step 4: Count per Year**
```csharp
.Select(g => new EmployeesByYearDto
{
    Year = g.Key,      // The year (from GroupBy)
    Count = g.Count()  // How many employees created that year
})
```

**Step 5: Sort Chronologically**
```csharp
.OrderBy(x => x.Year)
```
Sort by year ascending (oldest first)

**Step 6: Execute Query**
```csharp
.ToListAsync()
```
Execute on database and return as list

### Example Result
| Year | Count |
|------|-------|
| 2022 | 10 |
| 2023 | 15 |
| 2024 | 25 |

### Note on Year Extraction
The `CreatedAt.Year` property automatically extracts the year:
```
CreatedAt = 2024-03-15 10:30:00
CreatedAt.Year = 2024
```

This is efficient because it's executed on the database server, not in memory.

---

## Query 3: GetDashboardStatsAsync()

### LINQ Code
```csharp
var totalEmployees = await _context.Employees.CountAsync();
var activeEmployees = await _context.Employees
    .Where(e => e.IsActive)
    .CountAsync();
var inactiveEmployees = totalEmployees - activeEmployees;
var totalRoles = await _context.Roles.CountAsync();

return new DashboardStatsDto
{
    TotalEmployees = totalEmployees,
    ActiveEmployees = activeEmployees,
    InactiveEmployees = inactiveEmployees,
    TotalRoles = totalRoles
};
```

### SQL Equivalent
```sql
-- Total employees
SELECT COUNT(*) as TotalEmployees FROM Employees

-- Active employees
SELECT COUNT(*) as ActiveEmployees FROM Employees WHERE IsActive = 1

-- Total roles
SELECT COUNT(*) as TotalRoles FROM Roles
```

### Step-by-Step Breakdown

**Count 1: Total Employees**
```csharp
var totalEmployees = await _context.Employees.CountAsync();
```
Count all employee records (active + inactive)

**Count 2: Active Employees**
```csharp
var activeEmployees = await _context.Employees
    .Where(e => e.IsActive)
    .CountAsync();
```
Count only employees where IsActive = true

**Calculate 3: Inactive Employees**
```csharp
var inactiveEmployees = totalEmployees - activeEmployees;
```
Simple math: total - active = inactive
- If total = 50, active = 45, then inactive = 5

**Count 4: Total Roles**
```csharp
var totalRoles = await _context.Roles.CountAsync();
```
Count all role records

**Return Result**
```csharp
return new DashboardStatsDto
{
    TotalEmployees = totalEmployees,
    ActiveEmployees = activeEmployees,
    InactiveEmployees = inactiveEmployees,
    TotalRoles = totalRoles
};
```

### Example Result
```
TotalEmployees: 50
ActiveEmployees: 45
InactiveEmployees: 5
TotalRoles: 3
```

---

## Key LINQ Concepts Used

### Include (Eager Loading)
```csharp
.Include(er => er.Role)      // Load Role data
.Include(er => er.Employee)  // Load Employee data
```
Loads related data in single query (Join)

### GroupBy
```csharp
.GroupBy(er => er.Role.RoleName)
```
Groups items by a key
- Creates collections of items with same key

### Select (Projection)
```csharp
.Select(g => new EmployeesByRoleDto { ... })
```
Transforms data into new shape (DTO)

### Where (Filter)
```csharp
.Where(e => e.IsActive)
```
Filters items based on condition

### OrderBy / OrderByDescending
```csharp
.OrderBy(x => x.Year)              // Ascending
.OrderByDescending(x => x.Count)   // Descending
```
Sorts results

### ToListAsync
```csharp
.ToListAsync()
```
Executes query on database asynchronously

---

## Performance Notes

### 1. Employees by Role
- Uses indexes on IsActive and RoleId
- GroupBy executed in database
- Efficient single query

### 2. Employees by Year
- Simple table scan with filter
- Year extraction happens in database
- No complex joins needed

### 3. Dashboard Stats
- 2 separate queries (for counts)
- Both execute asynchronously
- Minimal overhead

---

## Filtering Behavior

### IsActive Filter
All queries include `IsActive = true` filter to:
- Exclude soft-deleted employees
- Show only current/active staff
- Maintain data integrity
- Consistent with business logic

### Example
With 50 total employees (45 active, 5 inactive):
- GroupBy queries only count 45
- Stats show both total and active
- Soft-deleted employees hidden from role/year analysis

---

## Sorting Behavior

### Employees by Role
```csharp
.OrderByDescending(x => x.Count)
```
**Result**: Developer (15), HR (10), Admin (5)  
**Purpose**: Largest groups first for visual prominence

### Employees by Year
```csharp
.OrderBy(x => x.Year)
```
**Result**: 2022, 2023, 2024  
**Purpose**: Chronological order for trends

---

## Using These Queries

### In Repository
```csharp
public async Task<IEnumerable<EmployeesByRoleDto>> GetEmployeesByRoleAsync()
{
    return await _context.EmployeeRoles
        .Include(er => er.Role)
        .Include(er => er.Employee)
        .Where(er => er.Employee.IsActive)
        .GroupBy(er => er.Role.RoleName)
        .Select(g => new EmployeesByRoleDto { ... })
        .OrderByDescending(x => x.Count)
        .ToListAsync();
}
```

### Called from Service
```csharp
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
```

### Called from Controller
```csharp
[HttpGet("employees-by-role")]
public async Task<ActionResult<ApiResponse<IEnumerable<EmployeesByRoleDto>>>> GetEmployeesByRole()
{
    try
    {
        var data = await _dashboardService.GetEmployeesByRoleAsync();
        return Ok(new ApiResponse<IEnumerable<EmployeesByRoleDto>> { ... });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new ApiResponse<object> { ... });
    }
}
```

---

## Customization Examples

### Show Only Specific Roles
```csharp
.Where(er => er.Employee.IsActive && 
             (er.Role.RoleName == "Developer" || 
              er.Role.RoleName == "HR"))
```

### Show Last 3 Years Only
```csharp
.Where(e => e.IsActive && 
            e.CreatedAt.Year >= DateTime.Now.Year - 2)
```

### Show Top 5 Roles
```csharp
.OrderByDescending(x => x.Count)
.Take(5)
```

### Filter by Department
```csharp
.Where(er => er.Employee.IsActive && 
             er.Employee.Department == "Engineering")
```

---

**Reference**: All queries in `Repositories/DashboardRepository.cs`
**Pattern**: LINQ to Entities with async/await
**Execution**: Database-side for performance
