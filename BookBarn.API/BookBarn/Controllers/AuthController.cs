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

        public AuthController(IAuthService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserDto registerUserDto)
        {
            var response = await _authenticationService.RegisterUserAsync(registerUserDto);

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

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _authenticationService.GetAllUsersAsync();
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
