using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IEmployeeService employeeService, ILogger<AuthController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        /// <summary>
        /// Login with email and password
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<EmployeeLoginDetailsDto>>> Login([FromBody] LoginDto loginRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Invalid login request",
                        Data = ModelState.Values.SelectMany(v => v.Errors)
                    });
                }

                var (success, message, employee) = await _employeeService.LoginAsync(loginRequest);

                if (!success)
                {
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = message
                    });
                }

                return Ok(new ApiResponse<EmployeeLoginDetailsDto>
                {
                    Success = true,
                    Message = message,
                    Data = employee
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error during login"
                });
            }
        }
    }
}
