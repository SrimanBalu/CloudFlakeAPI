# Role Management API - Testing Guide

## Testing with Postman

### 1. Collection Setup

Create a new Postman collection called "Role Management API"

Set base URL variable:
```
{{base_url}} = http://localhost:5000
```

### 2. Test Cases

#### Test 1: Get All Roles
```
METHOD: GET
URL: {{base_url}}/api/roles
Headers: None (GET request)

Expected Response: 200 OK
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

#### Test 2: Get Roles with Employee Count
```
METHOD: GET
URL: {{base_url}}/api/roles/with-count
Headers: None

Expected Response: 200 OK
{
  "success": true,
  "message": "Roles with employee count retrieved successfully",
  "data": [
    {"id": 1, "roleName": "Admin", "employeeCount": 2},
    {"id": 2, "roleName": "HR", "employeeCount": 5},
    {"id": 3, "roleName": "Developer", "employeeCount": 10}
  ]
}
```

#### Test 3: Get Single Role
```
METHOD: GET
URL: {{base_url}}/api/roles/1
Headers: None

Expected Response: 200 OK
{
  "success": true,
  "message": "Role retrieved successfully",
  "data": {"id": 1, "roleName": "Admin"}
}
```

#### Test 4: Get Non-existent Role
```
METHOD: GET
URL: {{base_url}}/api/roles/999
Headers: None

Expected Response: 404 Not Found
{
  "success": false,
  "message": "Role not found"
}
```

#### Test 5: Create Role
```
METHOD: POST
URL: {{base_url}}/api/roles
Headers: 
  Content-Type: application/json

Body (raw JSON):
{
  "roleName": "Manager"
}

Expected Response: 201 Created
{
  "success": true,
  "message": "Role created successfully",
  "data": {"id": 4, "roleName": "Manager"}
}
```

#### Test 6: Create Role with Duplicate Name
```
METHOD: POST
URL: {{base_url}}/api/roles
Headers: 
  Content-Type: application/json

Body (raw JSON):
{
  "roleName": "Admin"
}

Expected Response: 400 Bad Request
{
  "success": false,
  "message": "Role name already exists"
}
```

#### Test 7: Create Role with Empty Name
```
METHOD: POST
URL: {{base_url}}/api/roles
Headers: 
  Content-Type: application/json

Body (raw JSON):
{
  "roleName": ""
}

Expected Response: 400 Bad Request
{
  "success": false,
  "message": "Role name must be between 2 and 50 characters"
}
```

#### Test 8: Update Role
```
METHOD: PUT
URL: {{base_url}}/api/roles/4
Headers: 
  Content-Type: application/json

Body (raw JSON):
{
  "roleName": "Senior Manager"
}

Expected Response: 200 OK
{
  "success": true,
  "message": "Role updated successfully",
  "data": {"id": 4, "roleName": "Senior Manager"}
}
```

#### Test 9: Update Non-existent Role
```
METHOD: PUT
URL: {{base_url}}/api/roles/999
Headers: 
  Content-Type: application/json

Body (raw JSON):
{
  "roleName": "Test"
}

Expected Response: 404 Not Found
{
  "success": false,
  "message": "Role not found"
}
```

#### Test 10: Delete Role (No Employees)
```
METHOD: DELETE
URL: {{base_url}}/api/roles/4
Headers: None

Expected Response: 200 OK
{
  "success": true,
  "message": "Role deleted successfully"
}
```

#### Test 11: Delete Role with Employees
```
METHOD: DELETE
URL: {{base_url}}/api/roles/1
Headers: None

Expected Response: 404 Not Found
{
  "success": false,
  "message": "Role not found or has associated employees"
}
```

#### Test 12: Delete Non-existent Role
```
METHOD: DELETE
URL: {{base_url}}/api/roles/999
Headers: None

Expected Response: 404 Not Found
{
  "success": false,
  "message": "Role not found or has associated employees"
}
```

## Testing with cURL

### Get All Roles
```bash
curl -X GET http://localhost:5000/api/roles
```

### Get Roles with Count
```bash
curl -X GET http://localhost:5000/api/roles/with-count
```

### Get Single Role
```bash
curl -X GET http://localhost:5000/api/roles/1
```

### Create Role
```bash
curl -X POST http://localhost:5000/api/roles \
  -H "Content-Type: application/json" \
  -d '{"roleName":"Manager"}'
```

### Update Role
```bash
curl -X PUT http://localhost:5000/api/roles/4 \
  -H "Content-Type: application/json" \
  -d '{"roleName":"Senior Manager"}'
```

### Delete Role
```bash
curl -X DELETE http://localhost:5000/api/roles/4
```

## Testing with Swagger/OpenAPI

1. Start the application
2. Navigate to `http://localhost:5000/swagger/index.html`
3. Expand "Roles" section
4. Click "Try it out" on any endpoint
5. Enter parameters and click "Execute"

## JavaScript/Fetch Testing

### Get All Roles
```javascript
fetch('http://localhost:5000/api/roles')
  .then(response => response.json())
  .then(data => console.log(data))
  .catch(error => console.error('Error:', error));
```

### Create Role
```javascript
fetch('http://localhost:5000/api/roles', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify({
    roleName: 'Manager'
  })
})
  .then(response => response.json())
  .then(data => console.log(data))
  .catch(error => console.error('Error:', error));
```

### Update Role
```javascript
fetch('http://localhost:5000/api/roles/4', {
  method: 'PUT',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify({
    roleName: 'Senior Manager'
  })
})
  .then(response => response.json())
  .then(data => console.log(data))
  .catch(error => console.error('Error:', error));
```

### Delete Role
```javascript
fetch('http://localhost:5000/api/roles/4', {
  method: 'DELETE'
})
  .then(response => response.json())
  .then(data => console.log(data))
  .catch(error => console.error('Error:', error));
```

## Test Scenarios

### Scenario 1: CRUD Operations
1. ? Create a new role "Manager"
2. ? Read the new role by ID
3. ? Update the role name to "Team Lead"
4. ? Read all roles (should show updated name)
5. ? Delete the role

### Scenario 2: Validation Tests
1. ? Try creating role with empty name ? Should fail (400)
2. ? Try creating role with duplicate name ? Should fail (400)
3. ? Try creating role with name > 50 chars ? Should fail (400)
4. ? Try getting non-existent role ? Should fail (404)
5. ? Try updating non-existent role ? Should fail (404)
6. ? Try deleting non-existent role ? Should fail (404)

### Scenario 3: Business Logic Tests
1. ? Get all roles - verify seeded roles exist (Admin, HR, Developer)
2. ? Get roles with count - verify employee counts are correct
3. ? Try deleting role with employees - should fail with appropriate message
4. ? Create new role and verify it appears in list
5. ? Delete role without employees - should succeed

### Scenario 4: Integration Tests
1. ? Create employee with role via `/api/employees`
2. ? Get employees and verify RoleName is returned
3. ? Get that role via `/api/roles` and verify employee count
4. ? Try deleting that role - should fail (has employee)
5. ? Delete the employee
6. ? Now delete the role - should succeed

## Expected Test Results Summary

| Test | Status | Expected Result |
|------|--------|-----------------|
| Get All Roles | ? PASS | 200 OK, all roles returned |
| Get Roles with Count | ? PASS | 200 OK, roles with counts |
| Get Single Role | ? PASS | 200 OK, specific role |
| Get Non-existent | ? PASS | 404 Not Found |
| Create Valid Role | ? PASS | 201 Created |
| Create Duplicate | ? PASS | 400 Bad Request |
| Create Empty Name | ? PASS | 400 Bad Request |
| Update Valid Role | ? PASS | 200 OK |
| Update Non-existent | ? PASS | 404 Not Found |
| Delete No Employees | ? PASS | 200 OK |
| Delete With Employees | ? PASS | 404 Not Found |
| Delete Non-existent | ? PASS | 404 Not Found |

## Troubleshooting

### Issue: 404 Not Found on any endpoint
**Solution**: 
- Verify API is running on correct port (5000)
- Check base URL is correct
- Verify controller is registered

### Issue: 500 Internal Server Error
**Solution**:
- Check application logs
- Verify database connection string
- Run migrations if needed

### Issue: CORS Error
**Solution**:
- CORS is configured for `http://localhost:3000`
- Ensure React app is on correct port
- Check CORS policy in `Program.cs`

### Issue: Role name not updating
**Solution**:
- Verify role exists (GET first)
- Check new name is not duplicate
- Verify JSON format is correct

## Performance Testing

### Load Testing (10 concurrent requests)
```bash
for i in {1..10}; do
  curl http://localhost:5000/api/roles &
done
wait
```

### Response Time Testing
```javascript
console.time('API Call');
fetch('http://localhost:5000/api/roles')
  .then(response => response.json())
  .then(data => console.timeEnd('API Call'));
```

## Test Data

### Initial Seeded Roles
```
ID: 1, Name: Admin
ID: 2, Name: HR
ID: 3, Name: Developer
```

### Sample Test Roles to Create
```json
{"roleName": "Manager"}
{"roleName": "Team Lead"}
{"roleName": "Analyst"}
{"roleName": "Consultant"}
{"roleName": "Intern"}
```

---

**Note**: Always test in development first before production deployment.
