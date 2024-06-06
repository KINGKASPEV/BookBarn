using AutoMapper;
using BookBarn.Application.DTOs.Cart;
using BookBarn.Application.Services.Interfaces;
using BookBarn.Domain.Entities;
using BookBarn.Domain;
using BookBarn.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace BookBarn.Application.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly IGenericRepository<Cart> _cartRepository;
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CartService> _logger;

        public CartService(IGenericRepository<Cart> cartRepository, IGenericRepository<Book> bookRepository, IMapper mapper, ILogger<CartService> logger)
        {
            _cartRepository = cartRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<CartDto>> AddCartAsync(CreateCartDto createCartDto)
        {
            try
            {
                var cart = _mapper.Map<Cart>(createCartDto);
                cart.Books = new List<Book>();

                foreach (var bookId in createCartDto.BookIds)
                {
                    var book = await _bookRepository.GetByIdAsync(bookId);
                    if (book != null)
                    {
                        cart.Books.Add(book);
                    }
                }

                await _cartRepository.AddAsync(cart);
                await _cartRepository.SaveChangesAsync();

                var cartDto = _mapper.Map<CartDto>(cart);
                return ApiResponse<CartDto>.Success(cartDto, "Cart created successfully", 201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a cart.");
                return ApiResponse<CartDto>.Failed(false, "An error occurred while creating the cart.", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<CartDto>> AddBookToCartAsync(string cartId, string bookId)
        {
            try
            {
                var cart = await _cartRepository.GetByIdAsync(cartId);
                if (cart == null)
                {
                    return ApiResponse<CartDto>.Failed(false, "Cart not found", 404, new List<string> { "Cart not found" });
                }

                var book = await _bookRepository.GetByIdAsync(bookId);
                if (book == null)
                {
                    return ApiResponse<CartDto>.Failed(false, "Book not found", 404, new List<string> { "Book not found" });
                }

                cart.Books.Add(book);
                _cartRepository.Update(cart);
                await _cartRepository.SaveChangesAsync();

                var cartDto = _mapper.Map<CartDto>(cart);
                return ApiResponse<CartDto>.Success(cartDto, "Book added to cart successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding book {bookId} to cart {cartId}.");
                return ApiResponse<CartDto>.Failed(false, "An error occurred while adding book to cart.", 500, new List<string> { ex.Message });
            }
        }


        public async Task<ApiResponse<bool>> DeleteCartAsync(string id)
        {
            try
            {
                var cart = await _cartRepository.GetByIdAsync(id);
                if (cart == null)
                {
                    return ApiResponse<bool>.Failed(false, "Cart not found", 404, new List<string> { "Cart not found" });
                }
                _cartRepository.Delete(cart);
                await _cartRepository.SaveChangesAsync();
                return ApiResponse<bool>.Success(true, "Cart deleted successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting the cart with id {id}.");
                return ApiResponse<bool>.Failed(false, "An error occurred while deleting the cart.", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<IEnumerable<CartDto>>> GetAllCartsAsync()
        {
            try
            {
                var carts = await _cartRepository.GetAllAsync();
                var cartDtos = _mapper.Map<IEnumerable<CartDto>>(carts);
                return ApiResponse<IEnumerable<CartDto>>.Success(cartDtos, "Carts retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all carts.");
                return ApiResponse<IEnumerable<CartDto>>.Failed(false, "An error occurred while retrieving the carts.", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<CartDto>> GetCartByIdAsync(string id)
        {
            try
            {
                var cart = await _cartRepository.GetByIdAsync(id);
                if (cart == null)
                {
                    return ApiResponse<CartDto>.Failed(false, "Cart not found", 404, new List<string> { "Cart not found" });
                }
                var cartDto = _mapper.Map<CartDto>(cart);
                return ApiResponse<CartDto>.Success(cartDto, "Cart retrieved successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving the cart with id {id}.");
                return ApiResponse<CartDto>.Failed(false, "An error occurred while retrieving the cart.", 500, new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<CartDto>> RemoveBookFromCartAsync(string cartId, string bookId)
        {
            try
            {
                var cart = await _cartRepository.GetByIdAsync(cartId);
                if (cart == null)
                {
                    return ApiResponse<CartDto>.Failed(false, "Cart not found", 404, new List<string> { "Cart not found" });
                }

                var bookToRemove = cart.Books.FirstOrDefault(b => b.Id == bookId);
                if (bookToRemove == null)
                {
                    return ApiResponse<CartDto>.Failed(false, "Book not found in cart", 404, new List<string> { "Book not found in cart" });
                }

                cart.Books.Remove(bookToRemove);
                _cartRepository.Update(cart);
                await _cartRepository.SaveChangesAsync();

                var cartDto = _mapper.Map<CartDto>(cart);
                return ApiResponse<CartDto>.Success(cartDto, "Book removed from cart successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while removing book {bookId} from cart {cartId}.");
                return ApiResponse<CartDto>.Failed(false, "An error occurred while removing book from cart.", 500, new List<string> { ex.Message });
            }
        }


        public async Task<ApiResponse<CartDto>> UpdateCartAsync(UpdateCartDto updateCartDto)
        {
            try
            {
                var cart = await _cartRepository.GetByIdAsync(updateCartDto.Id);
                if (cart == null)
                {
                    return ApiResponse<CartDto>.Failed(false, "Cart not found", 404, new List<string> { "Cart not found" });
                }

                cart.Books.Clear();

                foreach (var bookId in updateCartDto.BookIds)
                {
                    var book = await _bookRepository.GetByIdAsync(bookId);
                    if (book != null)
                    {
                        cart.Books.Add(book);
                    }
                }

                _cartRepository.Update(cart);
                await _cartRepository.SaveChangesAsync();

                var cartDto = _mapper.Map<CartDto>(cart);
                return ApiResponse<CartDto>.Success(cartDto, "Cart updated successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating the cart with id {updateCartDto.Id}.");
                return ApiResponse<CartDto>.Failed(false, "An error occurred while updating the cart.", 500, new List<string> { ex.Message });
            }
        }
    }
}
