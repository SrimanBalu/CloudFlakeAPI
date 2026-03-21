using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Employee>>>> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                return Ok(new ApiResponse<IEnumerable<Employee>>
                {
                    Success = true,
                    Message = "Employees retrieved successfully",
                    Data = employees
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employees");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error retrieving employees"
                });
            }
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Employee>>> GetEmployeeById(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Employee not found"
                    });
                }

                return Ok(new ApiResponse<Employee>
                {
                    Success = true,
                    Message = "Employee retrieved successfully",
                    Data = employee
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving employee by ID");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error retrieving employee"
                });
            }
        }

        /// <summary>
        /// Add a new employee
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Employee>>> AddEmployee([FromBody] Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Invalid employee data",
                        Data = ModelState.Values.SelectMany(v => v.Errors)
                    });
                }

                var (success, message, addedEmployee) = await _employeeService.AddEmployeeAsync(employee);

                if (!success)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = message
                    });
                }

                return CreatedAtAction(nameof(GetEmployeeById), new { id = addedEmployee!.Id }, new ApiResponse<Employee>
                {
                    Success = true,
                    Message = message,
                    Data = addedEmployee
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding employee");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error adding employee"
                });
            }
        }

        /// <summary>
        /// Update an employee
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Employee>>> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Invalid employee data",
                        Data = ModelState.Values.SelectMany(v => v.Errors)
                    });
                }

                var (success, message, updatedEmployee) = await _employeeService.UpdateEmployeeAsync(id, employee);

                if (!success)
                {
                    if (message == "Employee not found")
                    {
                        return NotFound(new ApiResponse<object>
                        {
                            Success = false,
                            Message = message
                        });
                    }

                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = message
                    });
                }

                return Ok(new ApiResponse<Employee>
                {
                    Success = true,
                    Message = message,
                    Data = updatedEmployee
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error updating employee"
                });
            }
        }

        /// <summary>
        /// Delete an employee
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteEmployee(int id)
        {
            try
            {
                var (success, message) = await _employeeService.DeleteEmployeeAsync(id);

                if (!success)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = message
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error deleting employee"
                });
            }
        }
    }
}
