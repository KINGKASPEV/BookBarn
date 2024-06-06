using BookBarn.Application.DTOs.Cart;
using BookBarn.Domain;

namespace BookBarn.Application.Services.Interfaces
{
    public interface ICartService
    {
        Task<ApiResponse<CartDto>> AddCartAsync(CreateCartDto createCartDto);
        Task<ApiResponse<CartDto>> AddBookToCartAsync(string cartId, string bookId);
        Task<ApiResponse<CartDto>> RemoveBookFromCartAsync(string cartId, string bookId);
        Task<ApiResponse<bool>> DeleteCartAsync(string id);
        Task<ApiResponse<IEnumerable<CartDto>>> GetAllCartsAsync();
        Task<ApiResponse<CartDto>> GetCartByIdAsync(string id);
        Task<ApiResponse<CartDto>> UpdateCartAsync(UpdateCartDto updateCartDto);
    }
}
