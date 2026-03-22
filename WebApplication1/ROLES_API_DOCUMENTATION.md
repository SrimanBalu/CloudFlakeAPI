# Roles Management API Documentation

## Overview
This document describes the separate Roles management system created for the Employee Management API. The role system has been implemented with complete separation of concerns using the Repository and Service pattern.

## Architecture

### Components Created

#### 1. **Models** (Already Existed)
- `Role.cs` - Master role table with Id and RoleName
- `EmployeeRole.cs` - Mapping table linking Employees to Roles

#### 2. **Repositories**
- `IRoleRepository.cs` - Interface for role data access
- `RoleRepository.cs` - Implementation with database operations

#### 3. **Services**
- `IRoleService.cs` - Interface for role business logic
- `RoleService.cs` - Implementation with validation and error handling

#### 4. **Controllers**
- `RolesController.cs` - REST API endpoints for role management

#### 5. **DTOs**
- `CreateRoleDto` - DTO for creating roles
- `UpdateRoleDto` - DTO for updating roles
- `RoleDto` - DTO for role responses
- `RoleWithEmployeeCountDto` - DTO with employee count included

## API Endpoints

### Base URL
```
/api/roles
```

### Endpoints

#### 1. Get All Roles
```
GET /api/roles
```
**Response:**
```json
{
  "success": true,
  "message": "Roles retrieved successfully",
  "data": [
    {
      "id": 1,
      "roleName": "Admin"
    },
    {
      "id": 2,
      "roleName": "HR"
    },
    {
      "id": 3,
      "roleName": "Developer"
    }
  ]
}
```

#### 2. Get All Roles with Employee Count
```
GET /api/roles/with-count
```
**Response:**
```json
{
  "success": true,
  "message": "Roles with employee count retrieved successfully",
  "data": [
    {
      "id": 1,
      "roleName": "Admin",
      "employeeCount": 2
    },
    {
      "id": 2,
      "roleName": "HR",
      "employeeCount": 5
    },
    {
      "id": 3,
      "roleName": "Developer",
      "employeeCount": 10
    }
  ]
}
```

#### 3. Get Role by ID
```
GET /api/roles/{id}
```
**Response:**
```json
{
  "success": true,
  "message": "Role retrieved successfully",
  "data": {
    "id": 1,
    "roleName": "Admin"
  }
}
```

#### 4. Create New Role
```
POST /api/roles
Content-Type: application/json

{
  "roleName": "Manager"
}
```
**Response (201 Created):**
```json
{
  "success": true,
  "message": "Role created successfully",
  "data": {
    "id": 4,
    "roleName": "Manager"
  }
}
```

#### 5. Update Role
```
PUT /api/roles/{id}
Content-Type: application/json

{
  "roleName": "Senior Developer"
}
```
**Response:**
```json
{
  "success": true,
  "message": "Role updated successfully",
  "data": {
    "id": 3,
    "roleName": "Senior Developer"
  }
}
```

#### 6. Delete Role
```
DELETE /api/roles/{id}
```
**Response:**
```json
{
  "success": true,
  "message": "Role deleted successfully"
}
```

**Note:** A role can only be deleted if no employees are assigned to it. Attempting to delete a role with employees will return a 404 error with message: "Role not found or has associated employees"

## Features

### ? Implemented Features

1. **Complete CRUD Operations**
   - Create new roles
   - Read all roles and individual roles
   - Update role names
   - Delete roles (with validation)

2. **Employee Count Tracking**
   - Dedicated endpoint to view how many employees are assigned to each role
   - Useful for reporting and analytics

3. **Validation & Error Handling**
   - Role name validation
   - Duplicate role name detection
   - Prevents deletion of roles with assigned employees
   - Comprehensive error messages

4. **Async/Await Pattern**
   - All operations are fully asynchronous
   - Improved performance and scalability

5. **Clean Architecture**
   - Separation of concerns
   - Repository pattern for data access
   - Service pattern for business logic
   - DTOs for data transfer

6. **Dependency Injection**
   - Registered in `Program.cs`
   - Loosely coupled design
   - Easy to test and extend

## Dependency Injection Setup

In `Program.cs`:
```csharp
// Add Dependency Injection
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
```

## Database Configuration

The role system uses Entity Framework Core with the following configuration in `AppDbContext.cs`:

```csharp
// Configure Role entity
modelBuilder.Entity<Role>(entity =>
{
    entity.HasKey(r => r.Id);
    entity.Property(r => r.RoleName)
        .IsRequired()
        .HasMaxLength(50);
});

// Seed initial roles
modelBuilder.Entity<Role>().HasData(
    new Role { Id = 1, RoleName = "Admin" },
    new Role { Id = 2, RoleName = "HR" },
    new Role { Id = 3, RoleName = "Developer" }
);
```

## Relationships

### Role to EmployeeRole
- **One-to-Many**: One Role can have many EmployeeRoles
- **Delete Behavior**: Restrict (prevents deletion if employees exist)

### Employee to EmployeeRole
- **One-to-Many**: One Employee can have one Role (enforced in service layer)
- **Delete Behavior**: Cascade (when employee is deleted, EmployeeRole is also deleted)

## Error Responses

### 400 Bad Request
```json
{
  "success": false,
  "message": "Role name already exists"
}
```

### 404 Not Found
```json
{
  "success": false,
  "message": "Role not found or has associated employees"
}
```

### 500 Internal Server Error
```json
{
  "success": false,
  "message": "Error [operation]: [exception message]"
}
```

## Business Logic Rules

1. **Role Name Uniqueness**: Role names must be unique (case-insensitive)
2. **One Role Per Employee**: Enforced in EmployeeService when creating/updating employees
3. **No Orphaned Roles**: Roles with assigned employees cannot be deleted
4. **Cascade Delete**: When an employee is deleted, their role assignment is also deleted

## Testing Examples

### Using cURL

**Get all roles:**
```bash
curl -X GET http://localhost:5000/api/roles
```

**Create a new role:**
```bash
curl -X POST http://localhost:5000/api/roles \
  -H "Content-Type: application/json" \
  -d '{"roleName":"Team Lead"}'
```

**Update a role:**
```bash
curl -X PUT http://localhost:5000/api/roles/4 \
  -H "Content-Type: application/json" \
  -d '{"roleName":"Senior Team Lead"}'
```

**Delete a role:**
```bash
curl -X DELETE http://localhost:5000/api/roles/4
```

### Using Postman

1. Import the collection with the API endpoints
2. Set the base URL to your API URL
3. Test each endpoint following the examples above

## Future Enhancements

1. **Role Permissions**: Add permission/access level to roles
2. **Role Hierarchy**: Implement role hierarchy (e.g., Admin > Manager > Developer)
3. **Audit Logging**: Track role creation, updates, and deletions
4. **Role Templates**: Create predefined role templates
5. **Bulk Operations**: Bulk create/update/delete roles
6. **Role Descriptions**: Add detailed descriptions to roles

## Notes

- All timestamps are in UTC
- Role names are case-sensitive for display but case-insensitive for uniqueness checks
- The API supports both synchronous HTTP calls and async processing
- CORS is configured to allow requests from `http://localhost:3000` (React app)
