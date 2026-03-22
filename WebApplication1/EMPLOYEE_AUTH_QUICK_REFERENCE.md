# Employee Authentication - Quick Reference

## ?? Quick Start

### Step 1: Run Migration
```powershell
Add-Migration AddEmployeeAuthFields
Update-Database
```

### Step 2: Create Employee with Password
```bash
curl -X POST http://localhost:5000/api/employees \
  -H "Content-Type: application/json" \
  -d '{
    "name": "John Doe",
    "age": 30,
    "department": "Engineering",
    "email": "john@example.com",
    "phone": "1234567890",
    "password": "SecurePass123",
    "roleId": 3,
    "isActive": true
  }'
```

### Step 3: Login
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "john@example.com",
    "password": "SecurePass123"
  }'
```

---

## ?? API Endpoints

| Method | Endpoint | Purpose | Auth Required |
|--------|----------|---------|---|
| GET | `/api/employees` | Get all active employees | No |
| GET | `/api/employees/{id}` | Get specific employee | No |
| POST | `/api/employees` | Create employee with password | No |
| PUT | `/api/employees/{id}` | Update employee | No |
| DELETE | `/api/employees/{id}` | Soft delete employee | No |
| POST | `/api/auth/login` | Employee login | No |

---

## ?? Request/Response Examples

### Create Employee (POST /api/employees)

**Request:**
```json
{
  "name": "John Doe",
  "age": 30,
  "department": "Engineering",
  "email": "john@example.com",
  "phone": "1234567890",
  "password": "SecurePass123",
  "roleId": 3,
  "isActive": true
}
```

**Success Response (201):**
```json
{
  "success": true,
  "message": "Employee added successfully",
  "data": {
    "id": 1,
    "name": "John Doe",
    "age": 30,
    "department": "Engineering",
    "email": "john@example.com",
    "phone": "1234567890",
    "roleName": "Developer",
    "roleId": 3,
    "isActive": true
  }
}
```

**Error (400):**
```json
{
  "success": false,
  "message": "Email already exists"
}
```

---

### Update Employee (PUT /api/employees/{id})

**Request (password optional):**
```json
{
  "name": "John Doe",
  "age": 31,
  "department": "Engineering",
  "email": "john@example.com",
  "phone": "1234567890",
  "password": null,
  "roleId": 3,
  "isActive": true
}
```

**Response (200):**
```json
{
  "success": true,
  "message": "Employee updated successfully",
  "data": { ... }
}
```

---

### Employee Login (POST /api/auth/login)

**Request:**
```json
{
  "email": "john@example.com",
  "password": "SecurePass123"
}
```

**Success Response (200):**
```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "roleName": "Developer"
  }
}
```

**Error Response (401):**
```json
{
  "success": false,
  "message": "Invalid email or password"
}
```

**Inactive Account (401):**
```json
{
  "success": false,
  "message": "Employee account is inactive"
}
```

---

### Delete Employee (DELETE /api/employees/{id})

**Response (200):**
```json
{
  "success": true,
  "message": "Employee deleted successfully"
}
```

**Note:** Sets `IsActive = false` (soft delete, record not removed)

---

## ? Validation Rules

### Employee Creation
| Field | Required | Min | Max | Format |
|-------|----------|-----|-----|--------|
| Name | Yes | 2 | 100 | Text |
| Age | Yes | 19 | 120 | Number |
| Department | Yes | 2 | 50 | Text |
| Email | Yes | - | 255 | Email (unique) |
| Phone | Yes | 10 | 10 | Digits only |
| Password | Yes | 6 | 255 | Any |
| RoleId | Yes | 1 | 999 | Exists in Roles table |
| IsActive | No | - | - | Boolean (default: true) |

### Login
| Field | Required | Format |
|-------|----------|--------|
| Email | Yes | Valid email |
| Password | Yes | 6+ characters |

---

## ??? DTOs Reference

### EmployeeDto (Create)
```csharp
public class EmployeeDto
{
    public string Name { get; set; }           // Required, 2-100 chars
    public int Age { get; set; }               // Required, 19-120
    public string Department { get; set; }     // Required, 2-50 chars
    public string Email { get; set; }          // Required, unique
    public string Phone { get; set; }          // Required, 10 digits
    public string Password { get; set; }       // Required, 6+ chars
    public int RoleId { get; set; }            // Required
    public bool IsActive { get; set; } = true; // Optional, default true
}
```

### EmployeeUpdateDto (Update)
```csharp
public class EmployeeUpdateDto
{
    public string Name { get; set; }           // Required
    public int Age { get; set; }               // Required
    public string Department { get; set; }     // Required
    public string Email { get; set; }          // Required
    public string Phone { get; set; }          // Required
    public string? Password { get; set; }      // Optional - null = don't update
    public int RoleId { get; set; }            // Required
    public bool IsActive { get; set; }         // Required
}
```

### LoginDto (Login)
```csharp
public class LoginDto
{
    public string Email { get; set; }    // Required
    public string Password { get; set; } // Required
}
```

### EmployeeResponseDto (Response)
```csharp
public class EmployeeResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Department { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string RoleName { get; set; }
    public int RoleId { get; set; }
    public bool IsActive { get; set; }
}
```

### EmployeeLoginDetailsDto (Login Response)
```csharp
public class EmployeeLoginDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string RoleName { get; set; }
}
```

---

## ?? Security Notes

### Current
- ? Passwords stored as plain text (development only)
- ? Email uniqueness enforced
- ? IsActive status checking

### Production Required
- ?? Implement BCrypt password hashing
- ?? Add JWT token authentication
- ?? Use HTTPS only
- ?? Add rate limiting on login
- ?? Add password reset flow
- ?? Add account lockout after failed attempts

---

## ?? Testing Snippets

### Postman Collection
```json
{
  "info": {
    "name": "Employee API",
    "description": "Employee Management API with Authentication"
  },
  "item": [
    {
      "name": "Create Employee",
      "request": {
        "method": "POST",
        "url": "{{baseUrl}}/api/employees",
        "body": {
          "mode": "raw",
          "raw": "{\n  \"name\": \"John Doe\",\n  \"age\": 30,\n  \"department\": \"Engineering\",\n  \"email\": \"john@example.com\",\n  \"phone\": \"1234567890\",\n  \"password\": \"SecurePass123\",\n  \"roleId\": 3,\n  \"isActive\": true\n}"
        }
      }
    },
    {
      "name": "Login",
      "request": {
        "method": "POST",
        "url": "{{baseUrl}}/api/auth/login",
        "body": {
          "mode": "raw",
          "raw": "{\n  \"email\": \"john@example.com\",\n  \"password\": \"SecurePass123\"\n}"
        }
      }
    }
  ]
}
```

---

## ?? Common Errors

| Error | Cause | Solution |
|-------|-------|----------|
| 400 "Email already exists" | Duplicate email | Use unique email |
| 400 "Password must be at least 6 characters" | Short password | Use 6+ char password |
| 401 "Invalid email or password" | Wrong credentials | Verify email & password |
| 401 "Employee account is inactive" | IsActive = false | Check employee status |
| 404 "Employee not found" | Invalid ID | Verify employee exists |

---

## ?? Field Details

### Password
- **Type**: String
- **Length**: 6-255 characters
- **Required**: Yes on create, optional on update
- **Storage**: Plain text (?? use BCrypt in production)
- **Validation**: Minimum 6 characters

### IsActive
- **Type**: Boolean
- **Default**: true
- **Purpose**: Soft delete functionality
- **API**: Set to false to deactivate employee

### Email
- **Type**: String
- **Unique**: Yes (database constraint)
- **Required**: Yes
- **Length**: Max 255 characters
- **Validation**: Valid email format

---

## ?? Migration Commands

### Create Migration
```bash
# Package Manager Console
Add-Migration AddEmployeeAuthFields

# .NET CLI
dotnet ef migrations add AddEmployeeAuthFields
```

### Apply Migration
```bash
# Package Manager Console
Update-Database

# .NET CLI
dotnet ef database update
```

### Verify Migration
```sql
-- Check columns
SELECT * FROM Employees LIMIT 1;

-- Check email index
SELECT * FROM sys.indexes WHERE name = 'IX_Employees_Email';
```

---

## ?? Help & Support

### Files Created
- `MIGRATION_GUIDE_AUTH_FIELDS.md` - Detailed migration guide
- `EMPLOYEE_AUTH_IMPLEMENTATION_SUMMARY.md` - Complete summary

### Key Changes
- ? Employee model: +2 fields (Password, IsActive)
- ? DTOs: +4 new/updated
- ? Repository: +2 methods
- ? Service: +1 method (LoginAsync)
- ? Controllers: Updated endpoints + AuthController logic
- ? Database: Unique email constraint

---

**Status**: ? Ready to Use
**Framework**: .NET 8
**Database**: SQL Server
**Pattern**: Repository + Service + Controller
