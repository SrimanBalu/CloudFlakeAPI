using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Login with username and password
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                {
                    return BadRequest(new LoginResponse
                    {
                        Success = false,
                        Message = "Username and password are required"
                    });
                }

                // Simple authentication check
                if (request.Username == "admin" && request.Password == "1234")
                {
                    return Ok(new LoginResponse
                    {
                        Success = true,
                        Message = "Login successful",
                        Token = GenerateSimpleToken()
                    });
                }

                return BadRequest(new LoginResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, new LoginResponse
                {
                    Success = false,
                    Message = "Error during login"
                });
            }
        }

        private string GenerateSimpleToken()
        {
            // Simple token generation (in production, use JWT)
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"admin:{DateTime.UtcNow:O}"));
        }
    }
}
