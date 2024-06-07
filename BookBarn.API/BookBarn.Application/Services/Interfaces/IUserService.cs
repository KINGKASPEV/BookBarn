using BookBarn.Application.DTOs.User;
using BookBarn.Domain;

namespace BookBarn.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<IEnumerable<UserResponseDto>>> GetAllUsersAsync();
    }
}
