# Employee Management System - ASP.NET Core Web API

A complete ASP.NET Core Web API built with Entity Framework Core (Code First) following layered architecture pattern.

## Architecture Overview

```
??? Models
?   ??? Employee.cs
??? Data
?   ??? AppDbContext.cs
??? Repositories
?   ??? IEmployeeRepository.cs
?   ??? EmployeeRepository.cs
??? Services
?   ??? IEmployeeService.cs
?   ??? EmployeeService.cs
??? Controllers
?   ??? EmployeesController.cs
?   ??? AuthController.cs
??? DTOs
    ??? Responses.cs
```

## Project Structure

### Layered Architecture
- **Controller Layer**: Handles HTTP requests/responses (EmployeesController, AuthController)
- **Service Layer**: Contains business logic and validation (EmployeeService)
- **Repository Layer**: Data access abstraction (EmployeeRepository)
- **Data Layer**: Entity Framework Core DbContext (AppDbContext)

## Database Setup

### Prerequisites
- SQL Server (Local or SQL Server Express)
- .NET 8 SDK

### Connection String
The connection string is configured in `appsettings.json`:
```json
"DefaultConnection": "Server=.;Database=CloudeFlake-Database;Trusted_Connection=True;TrustServerCertificate=True;"
```

### Create Database with Migrations

#### Step 1: Add Initial Migration
Open Package Manager Console and run:
```powershell
Add-Migration InitialCreate
```

#### Step 2: Update Database
```powershell
Update-Database
```

### Alternative: Using .NET CLI
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## API Endpoints

### Authentication
- **POST** `/api/auth/login` - Login with username and password
  - Default credentials: username = "admin", password = "1234"

### Employee Management
- **GET** `/api/employees` - Get all employees (HTTP 200)
- **GET** `/api/employees/{id}` - Get employee by ID (HTTP 200/404)
- **POST** `/api/employees` - Create new employee (HTTP 201/400)
- **PUT** `/api/employees/{id}` - Update employee (HTTP 200/400/404)
- **DELETE** `/api/employees/{id}` - Delete employee (HTTP 200/404)

## Employee Model

```csharp
public class Employee
{
    public int Id { get; set; }                           // Auto-increment primary key
    public string Name { get; set; }                      // Required, 2-100 chars
    public int Age { get; set; }                          // Required, > 18
    public string Department { get; set; }                // Required, 2-50 chars
    public string Email { get; set; }                     // Required, valid email format
    public string Phone { get; set; }                     // Required, exactly 10 digits
    public DateTime CreatedAt { get; set; }               // Auto-set timestamp
    public DateTime? UpdatedAt { get; set; }              // Updated timestamp
}
```

## Validation Rules

| Field | Rules |
|-------|-------|
| Name | Required, 2-100 characters |
| Age | Required, must be > 18 |
| Department | Required, 2-50 characters |
| Email | Required, valid email format |
| Phone | Required, exactly 10 digits |

## HTTP Status Codes

- **200 OK** - Successful GET, PUT, DELETE
- **201 Created** - Successful POST
- **400 Bad Request** - Validation errors or invalid data
- **404 Not Found** - Resource not found
- **500 Internal Server Error** - Server error

## CORS Configuration

CORS is enabled for React frontend:
- **Allowed Origin**: `http://localhost:3000`
- **Allowed Methods**: All (GET, POST, PUT, DELETE)
- **Allowed Headers**: All
- **Credentials**: Allowed

## Testing

### Swagger UI
Access the Swagger documentation at: `https://localhost:5001/swagger/index.html`

### Example Requests

#### Login
```json
POST /api/auth/login
{
  "username": "admin",
  "password": "1234"
}
```

#### Add Employee
```json
POST /api/employees
{
  "name": "John Doe",
  "age": 30,
  "department": "IT",
  "email": "john@example.com",
  "phone": "9876543210"
}
```

#### Update Employee
```json
PUT /api/employees/1
{
  "name": "John Smith",
  "age": 31,
  "department": "HR",
  "email": "john.smith@example.com",
  "phone": "9876543211"
}
```

## NuGet Packages

- `Microsoft.EntityFrameworkCore.SqlServer` - EF Core SQL Server provider
- `Microsoft.EntityFrameworkCore.Tools` - Migration tools
- `Swashbuckle.AspNetCore` - Swagger/OpenAPI support

## Running the Application

1. **Restore NuGet packages**:
   ```bash
   dotnet restore
   ```

2. **Create database**:
   ```bash
   dotnet ef database update
   ```

3. **Run the application**:
   ```bash
   dotnet run
   ```

4. **Access Swagger UI**:
   Open browser and navigate to: `https://localhost:5001/swagger/index.html`

## Notes

- All timestamps are in UTC
- The phone number must be exactly 10 digits (no special characters)
- Email validation follows RFC 5322 standards
- The API uses async/await for all database operations
- Proper error handling and logging is implemented
- All endpoints return standardized API responses

## Future Enhancements

- Implement JWT authentication
- Add pagination for employee list
- Add filtering and searching capabilities
- Add role-based authorization
- Implement audit logging
- Add input sanitization
- Implement rate limiting
