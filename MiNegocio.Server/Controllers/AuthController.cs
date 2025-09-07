using Microsoft.AspNetCore.Mvc;
using MiNegocio.Server.Interfaces;
using MiNegocio.Server.Services;
using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Dto.Response;
using MiNegocio.Shared.Models;

namespace MiNegocio.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Role = UserRole.SystemUser,
                CompanyId = request.CompanyId
            };

            var response = await _authService.RegisterAsync(user, request.Password);
            return Ok(response);
        }
    }


}

