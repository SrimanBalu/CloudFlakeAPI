# Employee Authentication Fields - Migration Guide

## Overview
This migration adds authentication and status fields to the Employee entity:
- **Password** (string, required, min 6 chars) - For employee login
- **IsActive** (bool, default true) - For soft delete functionality

---

## Step 1: Create Migration

Run this command in the Package Manager Console:

```powershell
Add-Migration AddEmployeeAuthFields
```

Or using .NET CLI:

```bash
dotnet ef migrations add AddEmployeeAuthFields
```

---

## Step 2: Update Database

Run this command to apply the migration:

```powershell
Update-Database
```

Or using .NET CLI:

```bash
dotnet ef database update
```

---

## Step 3: Migration Changes

The migration will:

1. **Add Password Column**
   - Type: NVARCHAR(255)
   - Nullable: NO
   - Default: Empty string

2. **Add IsActive Column**
   - Type: BIT
   - Nullable: NO
   - Default: 1 (true)

3. **Add Unique Index on Email**
   - Ensures email uniqueness
   - Prevents duplicate employee registrations

---

## Step 4: Verify Migration

After running `Update-Database`, verify:

1. New columns exist in the Employees table:
   ```sql
   SELECT * FROM Employees
   ```

2. Check Email index is unique:
   ```sql
   SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('Employees')
   ```

---

## API Changes

### Create Employee (POST)
**Endpoint**: `POST /api/employees`

**Request Body**:
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

**Response** (201 Created):
```json
{
  "success": true,
  "message": "Employee added successfully",
  "data": {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "roleName": "Developer",
    "isActive": true
  }
}
```

### Update Employee (PUT)
**Endpoint**: `PUT /api/employees/{id}`

**Request Body** (Password is optional):
```json
{
  "name": "John Doe",
  "age": 31,
  "department": "Engineering",
  "email": "john@example.com",
  "phone": "1234567890",
  "password": "NewPassword123",
  "roleId": 3,
  "isActive": true
}
```

### Employee Login (POST)
**Endpoint**: `POST /api/auth/login`

**Request Body**:
```json
{
  "email": "john@example.com",
  "password": "SecurePass123"
}
```

**Response** (200 OK):
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

**Response** (401 Unauthorized):
```json
{
  "success": false,
  "message": "Invalid email or password"
}
```

### Delete Employee (Soft Delete)
**Endpoint**: `DELETE /api/employees/{id}`

**Response** (200 OK):
```json
{
  "success": true,
  "message": "Employee deleted successfully"
}
```

---

## Important Notes

### 1. Password Handling
?? **SECURITY WARNING**: Currently passwords are stored as plain text.

**For Production**, implement BCrypt hashing:
```csharp
// Example using BCrypt.Net-Next NuGet package
using BCrypt.Net;

// Hash password on create
employee.Password = BCrypt.Net.BCrypt.HashPassword(employeeDto.Password);

// Verify on login
bool isValidPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, employee.Password);
```

### 2. Soft Delete
The `IsActive` field enables soft deletes:
- **DELETE** request sets `IsActive = false`
- Record is NOT removed from database
- `GET` endpoints only return active employees
- Maintains data integrity for auditing

### 3. Email Uniqueness
- Unique index ensures no duplicate emails
- Prevents multiple accounts with same email
- Migration handles existing data validation

---

## Troubleshooting

### Migration Conflict
If you have existing employees without passwords:

**Option 1**: Delete all employees first
```sql
DELETE FROM EmployeeRoles;
DELETE FROM Employees;
```

**Option 2**: Provide default password during migration
The migration will use empty string as default, then update manually.

### Index Already Exists
If you get an error about email index:
```sql
DROP INDEX IX_Employees_Email ON Employees;
```
Then re-run migration.

---

## Rollback Migration (if needed)

To undo this migration:

```powershell
Remove-Migration
```

Or specify a target migration:

```powershell
Update-Database -Migration PreviousMigrationName
```

---

## Next Steps

1. ? Run migration
2. ? Test employee creation with password
3. ? Test login functionality
4. ? Implement BCrypt hashing for production
5. ? Add JWT tokens for stateless authentication
6. ? Add role-based authorization

---

**Migration Name**: AddEmployeeAuthFields
**Framework**: .NET 8
**ORM**: Entity Framework Core
**Database**: SQL Server
