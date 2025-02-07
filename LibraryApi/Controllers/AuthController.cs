using LibraryApi.Application.DTOs;
using LibraryApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            return result ? Ok("Registration success") : BadRequest("Registration error");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Application.DTOs.LoginRequest request)
        {
            var token = await _authService.AuthenticateAsync(request.Username, request.Password);
            return Ok(new { accessToken = token });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var newToken = await _authService.RefreshTokenAsync(request.RefreshToken);
            return Ok(new { accessToken = newToken });
        }
    }
}
