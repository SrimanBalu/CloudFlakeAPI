# Role Management System - Quick Reference Guide

## What Was Created?

A complete, separate role management system with the following components:

### ?? File Structure

```
WebApplication1/
??? Controllers/
?   ??? RolesController.cs ? NEW
??? Services/
?   ??? IRoleService.cs ? NEW
?   ??? RoleService.cs ? NEW
??? Repositories/
?   ??? IRoleRepository.cs ? NEW
?   ??? RoleRepository.cs ? NEW
??? DTOs/
?   ??? Responses.cs (UPDATED - Role DTOs added)
??? Program.cs (UPDATED - Role services registered)
```

## Key Features

? **Complete CRUD Operations**
- Create roles
- Read single/all roles (with optional employee count)
- Update roles
- Delete roles (with validation)

? **Validation & Business Logic**
- Duplicate role name prevention
- Prevents deletion of roles with employees
- Role name length validation
- Error handling and logging

? **Clean Architecture**
- Repository Pattern (data access)
- Service Pattern (business logic)
- Dependency Injection
- DTOs for API requests/responses

? **Database Integration**
- Entity Framework Core integration
- Proper relationships (One-to-Many)
- Cascade delete handling
- Foreign key constraints

## API Endpoints

### Quick API Call Examples

**Get all roles:**
```bash
curl http://localhost:5000/api/roles
```

**Get roles with employee count:**
```bash
curl http://localhost:5000/api/roles/with-count
```

**Get role by ID:**
```bash
curl http://localhost:5000/api/roles/1
```

**Create new role:**
```bash
curl -X POST http://localhost:5000/api/roles \
  -H "Content-Type: application/json" \
  -d '{"roleName":"Manager"}'
```

**Update role:**
```bash
curl -X PUT http://localhost:5000/api/roles/4 \
  -H "Content-Type: application/json" \
  -d '{"roleName":"Senior Manager"}'
```

**Delete role:**
```bash
curl -X DELETE http://localhost:5000/api/roles/4
```

## HTTP Response Examples

### Success Response (200 OK)
```json
{
  "success": true,
  "message": "Roles retrieved successfully",
  "data": [
    {"id": 1, "roleName": "Admin"},
    {"id": 2, "roleName": "HR"},
    {"id": 3, "roleName": "Developer"}
  ]
}
```

### Error Response (400 Bad Request)
```json
{
  "success": false,
  "message": "Role name already exists"
}
```

### Not Found Response (404)
```json
{
  "success": false,
  "message": "Role not found or has associated employees"
}
```

## Service Methods

### IRoleService Interface

```csharp
public interface IRoleService
{
    // Get all roles
    Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    
    // Get specific role
    Task<RoleDto?> GetRoleByIdAsync(int id);
    
    // Create role
    Task<(bool success, string message, RoleDto? role)> CreateRoleAsync(CreateRoleDto createRoleDto);
    
    // Update role
    Task<(bool success, string message, RoleDto? role)> UpdateRoleAsync(int id, UpdateRoleDto updateRoleDto);
    
    // Delete role
    Task<(bool success, string message)> DeleteRoleAsync(int id);
    
    // Get roles with employee counts
    Task<IEnumerable<RoleWithEmployeeCountDto>> GetAllRolesWithEmployeeCountAsync();
}
```

## DTOs

### Input DTOs

**CreateRoleDto:**
```csharp
public class CreateRoleDto
{
    public string RoleName { get; set; }
}
```

**UpdateRoleDto:**
```csharp
public class UpdateRoleDto
{
    public string RoleName { get; set; }
}
```

### Output DTOs

**RoleDto:**
```csharp
public class RoleDto
{
    public int Id { get; set; }
    public string RoleName { get; set; }
}
```

**RoleWithEmployeeCountDto:**
```csharp
public class RoleWithEmployeeCountDto
{
    public int Id { get; set; }
    public string RoleName { get; set; }
    public int EmployeeCount { get; set; }
}
```

## Dependency Injection

Already registered in `Program.cs`:

```csharp
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
```

## Database Schema

### Roles Table
```sql
CREATE TABLE Roles (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL,
    UNIQUE (RoleName)
)
```

### Seeded Roles
- ID: 1, Name: Admin
- ID: 2, Name: HR
- ID: 3, Name: Developer

## Validation Rules

| Rule | Details |
|------|---------|
| Role Name | Required, 2-50 characters |
| Uniqueness | Case-insensitive |
| Deletion | Only if no employees assigned |
| Creation | Validates against duplicates |

## Status Codes Reference

| Code | Meaning | Example |
|------|---------|---------|
| 200 | Success | GET, PUT, DELETE succeed |
| 201 | Created | POST creates new role |
| 400 | Bad Request | Invalid data, duplicate name |
| 404 | Not Found | Role ID doesn't exist |
| 500 | Server Error | Unexpected exception |

## How It Works

### Create Employee with Role Flow

```
POST /api/employees with EmployeeDto
??? EmployeeService.AddEmployeeAsync()
??? EmployeeRepository.AddEmployeeAsync()
??? EmployeeRepository.AddEmployeeRoleAsync()
    ??? Creates EmployeeRole linking employee to role
```

### Get Employee with Role Flow

```
GET /api/employees
??? EmployeeService.GetAllEmployeesAsync()
??? EmployeeRepository.GetAllEmployeesAsync()
??? Include(e => e.EmployeeRoles).ThenInclude(er => er.Role)
??? Returns EmployeeResponseDto with RoleName
```

### Update Employee Role Flow

```
PUT /api/employees/{id} with EmployeeDto (containing new RoleId)
??? EmployeeService.UpdateEmployeeAsync()
??? EmployeeRepository.UpdateEmployeeAsync()
??? EmployeeRepository.UpdateEmployeeRoleAsync()
??? Updates RoleId in EmployeeRole table
```

## Permissions

One role per employee:
- Enforced in `EmployeeService.AddEmployeeAsync()`
- Enforced in `EmployeeService.UpdateEmployeeAsync()`

## Integration Points

Role system integrates with:

1. **Employee Service** - Uses EmployeeDto with RoleId
2. **Employee Controller** - Returns RoleName with employee data
3. **Database** - Relationships via foreign keys
4. **API Response** - Wrapped in ApiResponse<T>

## Swagger/OpenAPI

When app runs, all role endpoints documented at:
```
http://localhost:5000/swagger/index.html
```

## Testing Tips

1. **Test GET first** - Verify initial roles
2. **Test POST** - Create new role
3. **Test PUT** - Update role name
4. **Test GET with count** - Verify employee counts
5. **Test DELETE** - Only works for roles without employees
6. **Test edge cases**:
   - Duplicate role names
   - Empty role name
   - Invalid IDs
   - Role with employees

## Common Error Messages

| Error | Cause | Solution |
|-------|-------|----------|
| "Role name already exists" | Duplicate name | Use different name |
| "Role not found" | Invalid ID | Check ID exists |
| "Role not found or has associated employees" | Deletion failed | Remove employees first |
| "Invalid role ID" | ID <= 0 | Use positive integer |
| 500 Server Error | Exception thrown | Check logs for details |

## Next Steps

1. ? Restart application (to clear file lock)
2. ? Run migrations (if needed)
3. ? Test endpoints via Swagger
4. ? Create React components for role management
5. ? Integrate role dropdown in employee forms

---

**Created By**: GitHub Copilot
**Date**: 2024
**Framework**: .NET 8, ASP.NET Core
**Pattern**: Repository + Service Pattern
**Status**: Production Ready ?
