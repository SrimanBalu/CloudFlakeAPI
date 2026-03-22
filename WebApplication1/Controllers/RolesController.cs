using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(IRoleService roleService, ILogger<RolesController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<RoleDto>>>> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(new ApiResponse<IEnumerable<RoleDto>>
                {
                    Success = true,
                    Message = "Roles retrieved successfully",
                    Data = roles
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving roles");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error retrieving roles"
                });
            }
        }

        /// <summary>
        /// Get all roles with employee count
        /// </summary>
        [HttpGet("with-count")]
        public async Task<ActionResult<ApiResponse<IEnumerable<RoleWithEmployeeCountDto>>>> GetAllRolesWithCount()
        {
            try
            {
                var roles = await _roleService.GetAllRolesWithEmployeeCountAsync();
                return Ok(new ApiResponse<IEnumerable<RoleWithEmployeeCountDto>>
                {
                    Success = true,
                    Message = "Roles with employee count retrieved successfully",
                    Data = roles
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving roles with count");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error retrieving roles"
                });
            }
        }

        /// <summary>
        /// Get role by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<RoleDto>>> GetRoleById(int id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Role not found"
                    });
                }

                return Ok(new ApiResponse<RoleDto>
                {
                    Success = true,
                    Message = "Role retrieved successfully",
                    Data = role
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving role by ID");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error retrieving role"
                });
            }
        }

        /// <summary>
        /// Create a new role
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<RoleDto>>> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Invalid role data",
                        Data = ModelState.Values.SelectMany(v => v.Errors)
                    });
                }

                var (success, message, role) = await _roleService.CreateRoleAsync(createRoleDto);

                if (!success)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = message
                    });
                }

                return CreatedAtAction(nameof(GetRoleById), new { id = role!.Id }, new ApiResponse<RoleDto>
                {
                    Success = true,
                    Message = message,
                    Data = role
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error creating role"
                });
            }
        }

        /// <summary>
        /// Update a role
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<RoleDto>>> UpdateRole(int id, [FromBody] UpdateRoleDto updateRoleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Invalid role data",
                        Data = ModelState.Values.SelectMany(v => v.Errors)
                    });
                }

                var (success, message, role) = await _roleService.UpdateRoleAsync(id, updateRoleDto);

                if (!success)
                {
                    if (message == "Role not found")
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

                return Ok(new ApiResponse<RoleDto>
                {
                    Success = true,
                    Message = message,
                    Data = role
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error updating role"
                });
            }
        }

        /// <summary>
        /// Delete a role (only if no employees are assigned)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteRole(int id)
        {
            try
            {
                var (success, message) = await _roleService.DeleteRoleAsync(id);

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
                _logger.LogError(ex, "Error deleting role");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Error deleting role"
                });
            }
        }
    }
}
