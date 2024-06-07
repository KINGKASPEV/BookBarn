using AutoMapper;
using BookBarn.Application.DTOs.Auth;
using BookBarn.Application.DTOs.User;
using BookBarn.Application.Services.Interfaces;
using BookBarn.Common.Utilities;
using BookBarn.Domain;
using BookBarn.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BookBarn.Application.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _config;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AuthService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<AuthService> logger,
            IConfiguration config, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _config = config;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<ApiResponse<UserResponseDto>> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            try
            {
                var user = _mapper.Map<AppUser>(registerUserDto);
                var existingUser = await _userManager.FindByEmailAsync(registerUserDto.Email);
                if (existingUser != null)
                    return ApiResponse<UserResponseDto>.Failed(false, "User with the specified email already exists.", 400, null);

                var result = await _userManager.CreateAsync(user, registerUserDto.Password);
                if (!result.Succeeded)
                    return ApiResponse<UserResponseDto>.Failed(false, "Failed to create user.", 500, result.Errors.Select(e => e.Description).ToList());

                if (!await _roleManager.RoleExistsAsync("User"))
                    await _roleManager.CreateAsync(new IdentityRole("User"));

                await _userManager.AddToRoleAsync(user, "User");
                var userResponseDto = _mapper.Map<UserResponseDto>(user);

                return ApiResponse<UserResponseDto>.Success(userResponseDto, "User registered successfully.", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering user.");
                return ApiResponse<UserResponseDto>.Failed(false, "An error occurred while registering user.", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(AppUserLoginDto loginDTO)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginDTO.Email);
                if (user == null)
                    return ApiResponse<LoginResponseDto>.Failed(false, "User not found.", StatusCodes.Status404NotFound, new List<string>());

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, lockoutOnFailure: false);

                switch (result)
                {
                    case { Succeeded: true }:
                        var jwtSettings = _config.GetSection("JwtSettings");
                        var jwtService = new JwtService(
                            jwtSettings["Secret"],
                            jwtSettings["ValidIssuer"],
                            jwtSettings["ValidAudience"]);

                        var roles = await _userManager.GetRolesAsync(user);
                        var userRoles = await Task.WhenAll(roles.Select(roleName => _roleManager.FindByNameAsync(roleName)));

                        var response = new LoginResponseDto
                        {
                            Id = user.Id,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            UserRole = userRoles.FirstOrDefault()?.Name,
                            JWToken = jwtService.GenerateToken(user.Id, user.Email, roles.ToArray())
                        };
                        return ApiResponse<LoginResponseDto>.Success(response, "Logged In Successfully", StatusCodes.Status200OK);

                    default:
                        return ApiResponse<LoginResponseDto>.Failed(false, "Login failed. Invalid email or password", StatusCodes.Status401Unauthorized, new List<string>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Some error occurred while logging in." + ex.Message);
                return ApiResponse<LoginResponseDto>.Failed(false, "Some error occurred while logging in." + ex.Message, StatusCodes.Status500InternalServerError, new List<string>() { ex.Message });
            }
        }

        public async Task<ApiResponse<string>> LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return new ApiResponse<string>(true, "User logged out successfully.", StatusCodes.Status200OK, null, new List<string>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging out");
                return new ApiResponse<string>(true, "Error occurred while logging out", StatusCodes.Status500InternalServerError, null, new List<string>() { ex.Message });
            }
        }
    }
}
