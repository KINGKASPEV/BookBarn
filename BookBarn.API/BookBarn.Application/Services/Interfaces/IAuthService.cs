using BookBarn.Application.DTOs.Auth;
using BookBarn.Application.DTOs.User;
using BookBarn.Domain;

namespace BookBarn.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<UserResponseDto>> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<ApiResponse<LoginResponseDto>> LoginAsync(AppUserLoginDto loginDTO);
        Task<ApiResponse<IEnumerable<UserResponseDto>>> GetAllUsersAsync();
        Task<ApiResponse<string>> LogoutAsync();
    }
}
