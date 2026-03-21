# Quick Start Guide

## Prerequisites
- .NET 8 SDK installed
- SQL Server running locally
- Visual Studio 2022 or Visual Studio Code

## Quick Setup (5 minutes)

### Step 1: Restore NuGet Packages
```powershell
cd WebApplication1
dotnet restore
```

### Step 2: Create Database
#### Option A: Package Manager Console (Visual Studio)
```powershell
Add-Migration InitialCreate
Update-Database
```

#### Option B: .NET CLI
```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Step 3: Run Application
```powershell
dotnet run
```

### Step 4: Access Swagger
Open browser and go to: `https://localhost:5001/swagger/index.html`

---

## API Quick Test Examples

### Test Login
```powershell
curl -X POST "https://localhost:5001/api/auth/login" `
  -H "Content-Type: application/json" `
  -d '{"username":"admin","password":"1234"}'
```

### Add Employee
```powershell
curl -X POST "https://localhost:5001/api/employees" `
  -H "Content-Type: application/json" `
  -d '{
    "name": "John Doe",
    "age": 30,
    "department": "IT",
    "email": "john@example.com",
    "phone": "9876543210"
  }'
```

### Get All Employees
```powershell
curl -X GET "https://localhost:5001/api/employees"
```

### Get Employee by ID
```powershell
curl -X GET "https://localhost:5001/api/employees/1"
```

### Update Employee
```powershell
curl -X PUT "https://localhost:5001/api/employees/1" `
  -H "Content-Type: application/json" `
  -d '{
    "name": "Jane Smith",
    "age": 28,
    "department": "HR",
    "email": "jane@example.com",
    "phone": "9876543211"
  }'
```

### Delete Employee
```powershell
curl -X DELETE "https://localhost:5001/api/employees/1"
```

---

## Project Structure

```
WebApplication1/
??? Controllers/
?   ??? EmployeesController.cs      # Employee endpoints
?   ??? AuthController.cs           # Authentication endpoint
??? Services/
?   ??? IEmployeeService.cs         # Service interface
?   ??? EmployeeService.cs          # Business logic
??? Repositories/
?   ??? IEmployeeRepository.cs      # Repository interface
?   ??? EmployeeRepository.cs       # Data access
??? Models/
?   ??? Employee.cs                 # Employee entity
??? Data/
?   ??? AppDbContext.cs             # EF Core DbContext
??? DTOs/
?   ??? Responses.cs                # API response models
??? Program.cs                      # Application configuration
??? appsettings.json                # Configuration file
??? README.md                       # Project documentation
??? SETUP_AND_DEPLOYMENT_GUIDE.md   # Detailed setup guide
??? QUICK_START.md                  # This file
```

---

## Common Commands

### Restore Packages
```bash
dotnet restore
```

### Build Project
```bash
dotnet build
```

### Run Project
```bash
dotnet run
```

### Add Migration
```bash
dotnet ef migrations add MigrationName
```

### Update Database
```bash
dotnet ef database update
```

### List Migrations
```bash
dotnet ef migrations list
```

### Remove Last Migration
```bash
dotnet ef migrations remove
```

---

## Validation Rules

| Field | Rules |
|-------|-------|
| Name | Required, 2-100 characters |
| Age | Required, minimum 19 years |
| Department | Required, 2-50 characters |
| Email | Required, valid email format |
| Phone | Required, exactly 10 digits |

---

## Response Status Codes

- `200 OK` - Successful GET/PUT/DELETE
- `201 Created` - Successful POST
- `400 Bad Request` - Validation error
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error

---

## Default Login Credentials

```
Username: admin
Password: 1234
```

---

## CORS Configuration

React frontend origin enabled: `http://localhost:3000`

To add more origins, edit `Program.cs` in the CORS policy section.

---

## Need Help?

1. Check `SETUP_AND_DEPLOYMENT_GUIDE.md` for troubleshooting
2. Access Swagger UI for API documentation
3. Review error logs in console output
4. Verify database connection string in `appsettings.json`

---

**Happy coding!** ??
