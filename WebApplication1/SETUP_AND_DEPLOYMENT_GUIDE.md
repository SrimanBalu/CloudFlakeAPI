# Employee Management System - Setup and Deployment Guide

## Overview

This document provides comprehensive instructions for setting up and running the Employee Management System ASP.NET Core Web API.

## System Requirements

- **Operating System**: Windows 10/11 or Windows Server 2019+
- **.NET SDK**: .NET 8.0 or later
- **SQL Server**: SQL Server 2019 or SQL Server Express 2019+
- **IDE**: Visual Studio 2022 or Visual Studio Code with C# extension

## Installation Steps

### Step 1: Verify .NET Installation

Open PowerShell and verify .NET 8 is installed:

```powershell
dotnet --version
```

Expected output: `8.0.x` or higher

### Step 2: Verify SQL Server Installation

Ensure SQL Server is running locally. To check available SQL Server instances:

```powershell
sqlcmd -L
```

### Step 3: Restore NuGet Packages

Navigate to the project directory and restore packages:

```powershell
cd WebApplication1
dotnet restore
```

## Database Setup

### Option 1: Using Package Manager Console (Visual Studio)

1. Open Visual Studio
2. Open **Package Manager Console** (Tools ? NuGet Package Manager ? Package Manager Console)
3. Run the following commands:

```powershell
# Add initial migration
Add-Migration InitialCreate

# Apply migration to database
Update-Database
```

### Option 2: Using .NET CLI

```powershell
# Navigate to project directory
cd WebApplication1

# Add initial migration
dotnet ef migrations add InitialCreate

# Apply migration to database
dotnet ef database update
```

### Verify Database Creation

After running migrations, verify the database was created in SQL Server:

1. Open SQL Server Management Studio (SSMS)
2. Connect to local server (`.` or `localhost`)
3. Look for database named `CloudeFlake-Database`
4. Expand and verify the `Employees` table exists

## Running the Application

### Using Visual Studio

1. Open the solution in Visual Studio 2022
2. Set **WebApplication1** as startup project
3. Press `F5` or click the **Run** button
4. Application will start on `https://localhost:5001`

### Using .NET CLI

```powershell
cd WebApplication1
dotnet run
```

Application will start and output:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
```

## Accessing the API

### Swagger UI

Once the application is running, access Swagger documentation:

**URL**: `https://localhost:5001/swagger/index.html`

Swagger provides an interactive interface to test all API endpoints.

### API Base URL

- **HTTPS**: `https://localhost:5001`
- **API Endpoint Base**: `https://localhost:5001/api`

## Testing API Endpoints

### 1. Authentication Endpoint

**Endpoint**: `POST /api/auth/login`

**Request Body**:
```json
{
  "username": "admin",
  "password": "1234"
}
```

**Expected Response** (200 OK):
```json
{
  "success": true,
  "message": "Login successful",
  "token": "YWRtaW46MjAyNC0wMS0xNVQwODozMDo1MC4xMjM0NTY3KzAwOjAw"
}
```

### 2. Get All Employees

**Endpoint**: `GET /api/employees`

**Expected Response** (200 OK):
```json
{
  "success": true,
  "message": "Employees retrieved successfully",
  "data": []
}
```

### 3. Add Employee

**Endpoint**: `POST /api/employees`

**Request Body**:
```json
{
  "name": "John Doe",
  "age": 30,
  "department": "IT",
  "email": "john@example.com",
  "phone": "9876543210"
}
```

**Expected Response** (201 Created):
```json
{
  "success": true,
  "message": "Employee added successfully",
  "data": {
    "id": 1,
    "name": "John Doe",
    "age": 30,
    "department": "IT",
    "email": "john@example.com",
    "phone": "9876543210",
    "createdAt": "2024-01-15T08:30:50.1234567Z",
    "updatedAt": null
  }
}
```

### 4. Get Employee by ID

**Endpoint**: `GET /api/employees/1`

**Expected Response** (200 OK):
```json
{
  "success": true,
  "message": "Employee retrieved successfully",
  "data": {
    "id": 1,
    "name": "John Doe",
    "age": 30,
    "department": "IT",
    "email": "john@example.com",
    "phone": "9876543210",
    "createdAt": "2024-01-15T08:30:50.1234567Z",
    "updatedAt": null
  }
}
```

### 5. Update Employee

**Endpoint**: `PUT /api/employees/1`

**Request Body**:
```json
{
  "name": "John Smith",
  "age": 31,
  "department": "HR",
  "email": "john.smith@example.com",
  "phone": "9876543211"
}
```

**Expected Response** (200 OK):
```json
{
  "success": true,
  "message": "Employee updated successfully",
  "data": {
    "id": 1,
    "name": "John Smith",
    "age": 31,
    "department": "HR",
    "email": "john.smith@example.com",
    "phone": "9876543211",
    "createdAt": "2024-01-15T08:30:50.1234567Z",
    "updatedAt": "2024-01-15T08:35:00.1234567Z"
  }
}
```

### 6. Delete Employee

**Endpoint**: `DELETE /api/employees/1`

**Expected Response** (200 OK):
```json
{
  "success": true,
  "message": "Employee deleted successfully",
  "data": null
}
```

## Validation Error Examples

### Invalid Age (Must be > 18)

**Response** (400 Bad Request):
```json
{
  "success": false,
  "message": "Age must be greater than 18"
}
```

### Invalid Email Format

**Response** (400 Bad Request):
```json
{
  "success": false,
  "message": "Invalid email format"
}
```

### Invalid Phone (Must be 10 digits)

**Response** (400 Bad Request):
```json
{
  "success": false,
  "message": "Phone must be exactly 10 digits"
}
```

### Employee Not Found

**Response** (404 Not Found):
```json
{
  "success": false,
  "message": "Employee not found"
}
```

## CORS Configuration

CORS is configured to allow requests from React frontend running on `http://localhost:3000`.

To add additional origins, edit `Program.cs`:

```csharp
options.AddPolicy("AllowReactApp",
    policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",
            "http://localhost:3001",  // Add additional origins here
            "https://yourdomain.com"
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
```

## Troubleshooting

### Issue: Database Connection Failed

**Error**: `A network-related or instance-specific error occurred while establishing a connection to SQL Server`

**Solution**:
1. Verify SQL Server is running
2. Check connection string in `appsettings.json`
3. Ensure Windows Authentication is enabled in SQL Server

### Issue: Migration Failed

**Error**: `Could not find a part of the path`

**Solution**:
1. Ensure you're in the correct directory (WebApplication1 folder)
2. Delete `bin` and `obj` folders
3. Run `dotnet clean` then `dotnet build`
4. Retry migrations

### Issue: Port Already in Use

**Error**: `Failed to bind to address https://localhost:5001`

**Solution**:

Either kill the process using port 5001:
```powershell
netstat -ano | findstr :5001
taskkill /PID <PID> /F
```

Or use a different port:
```powershell
dotnet run --urls="https://localhost:5002"
```

### Issue: CORS Error

**Error**: `Access to XMLHttpRequest at 'https://localhost:5001/api/...' from origin 'http://localhost:3000' has been blocked by CORS policy`

**Solution**:
1. Verify CORS is enabled in `Program.cs`
2. Check the origin matches exactly in CORS policy
3. Ensure `app.UseCors()` is called before `app.MapControllers()`

## Production Deployment Considerations

### Security Recommendations

1. **Use JWT for Authentication**: Replace the simple token generation with JWT tokens
2. **Add HTTPS**: Ensure SSL/TLS certificate is properly configured
3. **Database Security**: Use SQL Server authentication instead of Trusted Connection
4. **Rate Limiting**: Implement API rate limiting
5. **Logging**: Configure structured logging (e.g., Serilog)
6. **Validation**: Add additional input sanitization

### Database Configuration for Production

Update `appsettings.json` for production:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=YourDatabase;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
  }
}
```

### Deployment Steps

1. Publish the application:
   ```powershell
   dotnet publish -c Release -o ./publish
   ```

2. Create application settings for production:
   ```powershell
   Copy-Item appsettings.json appsettings.Production.json
   ```

3. Update database connection string in `appsettings.Production.json`

4. Deploy to hosting platform (Azure, IIS, Docker, etc.)

## Performance Optimization

### Pagination (Future Enhancement)

Implement pagination for large datasets:

```csharp
public async Task<(List<Employee> employees, int total)> GetEmployeesAsync(int pageNumber, int pageSize)
{
    var total = await _context.Employees.CountAsync();
    var employees = await _context.Employees
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    return (employees, total);
}
```

### Caching

Add response caching:

```csharp
[HttpGet]
[ResponseCache(Duration = 300)] // Cache for 5 minutes
public async Task<ActionResult<ApiResponse<IEnumerable<Employee>>>> GetAllEmployees()
{
    // ...
}
```

## Support and Documentation

- **Official ASP.NET Core Docs**: https://learn.microsoft.com/en-us/aspnet/core/
- **Entity Framework Core**: https://learn.microsoft.com/en-us/ef/core/
- **Swagger/OpenAPI**: https://swagger.io/

## Additional Resources

- Project repository structure follows clean architecture principles
- All code includes proper error handling and logging
- API responses follow REST conventions
- Database uses Code First migrations for version control

---

**Last Updated**: 2024
**API Version**: 1.0
