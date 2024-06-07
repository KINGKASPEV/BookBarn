using BookBarn.Application.DTOs.Auth;
using BookBarn.Application.DTOs.User;
using BookBarn.Application.Services.Implementations;
using BookBarn.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookBarn.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authenticationService;

        public AuthController(AuthService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUserAdmin([FromBody] RegisterUserDto createUserAdminDto)
        {
            var response = await _authenticationService.RegisterUserAsync(createUserAdminDto);

            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AppUserLoginDto loginDTO)
        {
            var response = await _authenticationService.LoginAsync(loginDTO);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var response = await _authenticationService.LogoutAsync();
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }
    }
}
