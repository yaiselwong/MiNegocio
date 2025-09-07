using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiNegocio.Server.Interfaces;
using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;
using MiNegocio.Shared.Mapper;

namespace MiNegocio.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userService;
        private readonly ICompanyManagementService _companyService;

        public UserManagementController(IUserManagementService userService, ICompanyManagementService companyService)
        {
            _userService = userService;
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            // Only admins can view all users
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var users = await _userService.GetAllUsersAsync();
            return Ok(users.ToDto());
        }

        [HttpGet("company/{companyId}")]
        public async Task<ActionResult<List<UserDto>>> GetUsersByCompany(int companyId)
        {
            // Admins can view users of any company
            // System users can only view users of their own company
            if (!User.IsInRole("Admin"))
            {
                var userCompanyId = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(userCompanyId) || int.Parse(userCompanyId) != companyId)
                {
                    return Forbid();
                }
            }

            var users = await _userService.GetUsersByCompanyAsync(companyId);
            return Ok(users.ToDto());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            // Admins can view any user
            // System users can only view users of their own company
            if (!User.IsInRole("Admin"))
            {
                var userCompanyId = User.FindFirst("CompanyId")?.Value;
                var user1 = await _userService.GetUserByIdAsync(id);
                if (user1 == null || string.IsNullOrEmpty(userCompanyId) || int.Parse(userCompanyId) != user1.CompanyId)
                {
                    return Forbid();
                }
            }

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserRequest request)
        {
            // Only admins can create users
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var user = await _userService.CreateUserAsync(request);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            // Only admins can update users
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var user = await _userService.UpdateUserAsync(request);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.ToDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Only admins can delete users
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var success = await _userService.DeleteUserAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost("{id}/reset-password")]
        public async Task<IActionResult> ResetPassword(int id, [FromBody] ResetPasswordRequest request)
        {
            // Only admins can reset passwords
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var success = await _userService.ResetPasswordAsync(id, request.NewPassword);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }



}
