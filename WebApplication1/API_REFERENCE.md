# Employee Management System - API Reference

## API Overview

Complete REST API for Employee Management System built with ASP.NET Core 8 and Entity Framework Core.

- **Base URL**: `https://localhost:5001/api`
- **API Version**: 1.0
- **Framework**: ASP.NET Core 8
- **Database**: SQL Server

---

## Authentication Endpoints

### POST /api/auth/login

Login and get authentication token.

**Request Body:**
```json
{
  "username": "string",
  "password": "string"
}
```

**Example Request:**
```bash
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"1234"}'
```

**Success Response (200 OK):**
```json
{
  "success": true,
  "message": "Login successful",
  "token": "YWRtaW46MjAyNC0wMS0xNVQwODozMDo1MC4xMjM0NTY3KzAwOjAw"
}
```

**Error Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "Invalid username or password"
}
```

**Default Credentials:**
- Username: `admin`
- Password: `1234`

---

## Employee Endpoints

### GET /api/employees

Get all employees.

**Request:**
```bash
curl -X GET "https://localhost:5001/api/employees"
```

**Success Response (200 OK):**
```json
{
  "success": true,
  "message": "Employees retrieved successfully",
  "data": [
    {
      "id": 1,
      "name": "John Doe",
      "age": 30,
      "department": "IT",
      "email": "john@example.com",
      "phone": "9876543210",
      "createdAt": "2024-01-15T08:30:50.1234567Z",
      "updatedAt": null
    },
    {
      "id": 2,
      "name": "Jane Smith",
      "age": 28,
      "department": "HR",
      "email": "jane@example.com",
      "phone": "9876543211",
      "createdAt": "2024-01-15T09:15:30.1234567Z",
      "updatedAt": null
    }
  ]
}
```

**Error Response (500 Internal Server Error):**
```json
{
  "success": false,
  "message": "Error retrieving employees"
}
```

---

### GET /api/employees/{id}

Get a specific employee by ID.

**Parameters:**
- `id` (path, required): Employee ID (integer)

**Request:**
```bash
curl -X GET "https://localhost:5001/api/employees/1"
```

**Success Response (200 OK):**
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

**Not Found Response (404 Not Found):**
```json
{
  "success": false,
  "message": "Employee not found"
}
```

---

### POST /api/employees

Create a new employee.

**Request Body:**
```json
{
  "name": "string (required, 2-100 chars)",
  "age": "integer (required, > 18)",
  "department": "string (required, 2-50 chars)",
  "email": "string (required, valid email)",
  "phone": "string (required, exactly 10 digits)"
}
```

**Example Request:**
```bash
curl -X POST "https://localhost:5001/api/employees" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Alice Johnson",
    "age": 28,
    "department": "Marketing",
    "email": "alice@example.com",
    "phone": "9876543212"
  }'
```

**Success Response (201 Created):**
```json
{
  "success": true,
  "message": "Employee added successfully",
  "data": {
    "id": 3,
    "name": "Alice Johnson",
    "age": 28,
    "department": "Marketing",
    "email": "alice@example.com",
    "phone": "9876543212",
    "createdAt": "2024-01-15T10:00:00.1234567Z",
    "updatedAt": null
  }
}
```

**Validation Error Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "Age must be greater than 18"
}
```

**Common Validation Errors:**
- Age: "Age must be greater than 18"
- Email: "Invalid email format"
- Phone: "Phone must be exactly 10 digits"
- Name: "Name must be between 2 and 100 characters"
- Department: "Department must be between 2 and 50 characters"

---

### PUT /api/employees/{id}

Update an existing employee.

**Parameters:**
- `id` (path, required): Employee ID (integer)

**Request Body:**
```json
{
  "name": "string (required, 2-100 chars)",
  "age": "integer (required, > 18)",
  "department": "string (required, 2-50 chars)",
  "email": "string (required, valid email)",
  "phone": "string (required, exactly 10 digits)"
}
```

**Example Request:**
```bash
curl -X PUT "https://localhost:5001/api/employees/1" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "John Smith",
    "age": 31,
    "department": "Finance",
    "email": "john.smith@example.com",
    "phone": "9876543213"
  }'
```

**Success Response (200 OK):**
```json
{
  "success": true,
  "message": "Employee updated successfully",
  "data": {
    "id": 1,
    "name": "John Smith",
    "age": 31,
    "department": "Finance",
    "email": "john.smith@example.com",
    "phone": "9876543213",
    "createdAt": "2024-01-15T08:30:50.1234567Z",
    "updatedAt": "2024-01-15T10:15:30.1234567Z"
  }
}
```

**Not Found Response (404 Not Found):**
```json
{
  "success": false,
  "message": "Employee not found"
}
```

**Validation Error Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "Invalid email format"
}
```

---

### DELETE /api/employees/{id}

Delete an employee.

**Parameters:**
- `id` (path, required): Employee ID (integer)

**Request:**
```bash
curl -X DELETE "https://localhost:5001/api/employees/1"
```

**Success Response (200 OK):**
```json
{
  "success": true,
  "message": "Employee deleted successfully",
  "data": null
}
```

**Not Found Response (404 Not Found):**
```json
{
  "success": false,
  "message": "Employee not found"
}
```

---

## HTTP Status Codes

| Code | Status | Description |
|------|--------|-------------|
| 200 | OK | Request succeeded |
| 201 | Created | Resource created successfully |
| 400 | Bad Request | Invalid request data or validation error |
| 404 | Not Found | Resource not found |
| 500 | Internal Server Error | Server error occurred |

---

## Request Headers

### Common Headers

```
Content-Type: application/json
Accept: application/json
User-Agent: PostmanRuntime/7.x
```

---

## Response Format

All API responses follow this standard format:

```json
{
  "success": "boolean (true/false)",
  "message": "string (success or error message)",
  "data": "object or array (null if error)"
}
```

---

## Field Validation Rules

### Employee Fields

#### Name
- **Required**: Yes
- **Type**: String
- **Min Length**: 2 characters
- **Max Length**: 100 characters
- **Validation**: Must not be empty or whitespace

#### Age
- **Required**: Yes
- **Type**: Integer
- **Min Value**: 19
- **Max Value**: 120
- **Validation**: Must be greater than 18

#### Department
- **Required**: Yes
- **Type**: String
- **Min Length**: 2 characters
- **Max Length**: 50 characters
- **Validation**: Must not be empty or whitespace

#### Email
- **Required**: Yes
- **Type**: String
- **Format**: Valid email address
- **Max Length**: 255 characters
- **Validation**: Must match email regex pattern (RFC 5322)

#### Phone
- **Required**: Yes
- **Type**: String
- **Length**: Exactly 10 digits
- **Format**: Numeric digits only
- **Validation**: Must be exactly 10 digits (e.g., "9876543210")

---

## Error Examples

### Example 1: Age Too Young
**Request:**
```json
{
  "name": "Bob Wilson",
  "age": 17,
  "department": "IT",
  "email": "bob@example.com",
  "phone": "9876543214"
}
```

**Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "Age must be greater than 18"
}
```

### Example 2: Invalid Email
**Request:**
```json
{
  "name": "Charlie Brown",
  "age": 25,
  "department": "HR",
  "email": "invalid-email",
  "phone": "9876543215"
}
```

**Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "Invalid email format"
}
```

### Example 3: Invalid Phone
**Request:**
```json
{
  "name": "Diana Prince",
  "age": 26,
  "department": "Sales",
  "email": "diana@example.com",
  "phone": "987654321"
}
```

**Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "Phone must be exactly 10 digits"
}
```

### Example 4: Employee Not Found
**Request:**
```
GET /api/employees/999
```

**Response (404 Not Found):**
```json
{
  "success": false,
  "message": "Employee not found"
}
```

---

## Using with Postman

### Import Collection Steps

1. Open Postman
2. Create new collection "Employee API"
3. Add requests with:
   - **GET** `/api/employees`
   - **GET** `/api/employees/{{id}}`
   - **POST** `/api/employees`
   - **PUT** `/api/employees/{{id}}`
   - **DELETE** `/api/employees/{{id}}`
   - **POST** `/api/auth/login`

### Environment Variables

Set these in Postman environment:
```
base_url: https://localhost:5001
api_path: /api
```

---

## Using with cURL Examples

### Get All
```bash
curl -X GET "https://localhost:5001/api/employees"
```

### Get By ID
```bash
curl -X GET "https://localhost:5001/api/employees/1"
```

### Create
```bash
curl -X POST "https://localhost:5001/api/employees" \
  -H "Content-Type: application/json" \
  -d '{"name":"Test","age":25,"department":"IT","email":"test@test.com","phone":"1234567890"}'
```

### Update
```bash
curl -X PUT "https://localhost:5001/api/employees/1" \
  -H "Content-Type: application/json" \
  -d '{"name":"Updated","age":26,"department":"HR","email":"updated@test.com","phone":"0987654321"}'
```

### Delete
```bash
curl -X DELETE "https://localhost:5001/api/employees/1"
```

### Login
```bash
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"1234"}'
```

---

## CORS Enabled Origins

The API accepts requests from:
- `http://localhost:3000` (React frontend)

To test from different origins, update `Program.cs`:

```csharp
options.AddPolicy("AllowReactApp",
    policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",
            "http://localhost:3001",
            "https://yourdomain.com"
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
```

---

## Rate Limiting (Recommended)

While not implemented in this version, add rate limiting for production:

```bash
Install-Package AspNetCoreRateLimit
```

---

## Pagination (Future Enhancement)

For large datasets, implement pagination:

```
GET /api/employees?pageNumber=1&pageSize=10
```

---

## Filtering & Search (Future Enhancement)

```
GET /api/employees?department=IT&minAge=25
```

---

## API Documentation

- **Swagger UI**: `https://localhost:5001/swagger/index.html`
- **OpenAPI Spec**: `https://localhost:5001/swagger/v1/swagger.json`

---

## Support & Contact

For issues or questions, refer to:
- **README.md** - Project overview
- **SETUP_AND_DEPLOYMENT_GUIDE.md** - Setup instructions
- **QUICK_START.md** - Quick reference

---

**API Version**: 1.0  
**Last Updated**: 2024  
**Status**: Production Ready
