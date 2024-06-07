using AutoMapper;
using BookBarn.Application.DTOs.User;
using BookBarn.Application.Services.Interfaces;
using BookBarn.Domain;
using BookBarn.Domain.Entities;
using BookBarn.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BookBarn.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<AppUser> _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<AppUser> userRepository, 
            UserManager<AppUser> userManager, ILogger<UserService> logger, 
            IMapper mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<UserResponseDto>>> GetAllUsersAsync()
        {
            try
            {
                var allUsers = await _userRepository.GetAllAsync();

                var userDtos = new List<UserResponseDto>();
                foreach (var user in allUsers)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var userDto = _mapper.Map<UserResponseDto>(user);
                    userDto.Role = roles.FirstOrDefault();
                    userDtos.Add(userDto);
                }

                return ApiResponse<IEnumerable<UserResponseDto>>.Success(userDtos, "Users retrieved successfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving users: {ex.Message}");
                return ApiResponse<IEnumerable<UserResponseDto>>.Failed(false, "An error occurred while retrieving users", StatusCodes.Status500InternalServerError, new List<string> { ex.Message });
            }
        }
    }
}
