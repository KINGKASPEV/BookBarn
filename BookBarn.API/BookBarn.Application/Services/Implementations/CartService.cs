using AutoMapper;
using BookBarn.Application.DTOs.Cart;
using BookBarn.Application.Services.Interfaces;
using BookBarn.Domain.Entities;
using BookBarn.Domain;
using BookBarn.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using BookBarn.Application.DTOs.Checkout;
using BookBarn.Application.DTOs.PurchaseHistory;

namespace BookBarn.Application.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly IGenericRepository<Cart> _cartRepository;
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IGenericRepository<Checkout> _checkoutRepository;
        private readonly IGenericRepository<PurchaseHistory> _purchaseHistoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CartService> _logger;

        public CartService(IGenericRepository<Cart> cartRepository,
            IGenericRepository<Book> bookRepository,
            IGenericRepository<Checkout> checkoutRepository,
            IGenericRepository<PurchaseHistory> purchaseHistoryRepository,
            IMapper mapper, ILogger<CartService> logger)
        {
            _cartRepository = cartRepository;
            _bookRepository = bookRepository;
            _checkoutRepository = checkoutRepository;
            _purchaseHistoryRepository = purchaseHistoryRepository;
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

        //public async Task<ApiResponse<IEnumerable<CartDto>>> GetAllCartsAsync()
        //{
        //    try
        //    {
        //        var carts = await _cartRepository.GetAllAsync();
        //        var cartDtos = _mapper.Map<IEnumerable<CartDto>>(carts);
        //        return ApiResponse<IEnumerable<CartDto>>.Success(cartDtos, "Carts retrieved successfully", 200);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while retrieving all carts.");
        //        return ApiResponse<IEnumerable<CartDto>>.Failed(false, "An error occurred while retrieving the carts.", 500, new List<string> { ex.Message });
        //    }
        //}

        public async Task<ApiResponse<IEnumerable<CartDto>>> GetAllCartsAsync()
        {
            try
            {
                var carts = await _cartRepository.GetAllToIcludeAsync(include => include.Books);
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
                var cart = await _cartRepository.GetByIdAsync(updateCartDto.CartId);
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
                _logger.LogError(ex, $"Error occurred while updating the cart with id {updateCartDto.CartId}.");
                return ApiResponse<CartDto>.Failed(false, "An error occurred while updating the cart.", 500, new List<string> { ex.Message });
            }
        }

        //public async Task<ApiResponse<CheckoutResponseDto>> CheckoutAsync(string cartId, CheckoutRequestDto checkoutRequestDto)
        //{
        //    try
        //    {
        //        var cart = await _cartRepository.GetByIdAsync(cartId);

        //        if (cart == null)
        //        {
        //            return ApiResponse<CheckoutResponseDto>.Failed(false, "Cart not found", 404, new List<string> { "Cart not found" });
        //        }

        //        string message = checkoutRequestDto.PaymentMethod switch
        //        {
        //            "Web" => "Payment processed via Web.",
        //            "USSD" => "Payment processed via USSD.",
        //            "Transfer" => "Payment processed via Transfer.",
        //            _ => "Invalid payment method."
        //        };

        //        if (message == "Invalid payment method.")
        //        {
        //            return ApiResponse<CheckoutResponseDto>.Failed(false, message, 400, new List<string> { message });
        //        }

        //        var checkout = new Checkout
        //        {
        //            CartId = cartId,
        //            AppUserId = cart.AppUserId, 
        //            PaymentMethod = checkoutRequestDto.PaymentMethod,
        //            Status = "Success",
        //            CompletedAt = DateTime.UtcNow,
        //            Message = message
        //        };

        //        await _checkoutRepository.AddAsync(checkout);
        //        await _checkoutRepository.SaveChangesAsync();

        //        cart.Books?.Clear();
        //        _cartRepository.Update(cart);
        //        await _cartRepository.SaveChangesAsync();

        //        var checkoutResponse = _mapper.Map<CheckoutResponseDto>(checkout);

        //        return ApiResponse<CheckoutResponseDto>.Success(checkoutResponse, "Checkout completed successfully", 200);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred during checkout.");
        //        return ApiResponse<CheckoutResponseDto>.Failed(false, "An error occurred during checkout.", 500, new List<string> { ex.Message });
        //    }
        //}

        //public async Task<ApiResponse<CheckoutResponseDto>> CheckoutAsync(string cartId, CheckoutRequestDto checkoutRequestDto)
        //{
        //    try
        //    {
        //        var cart = await _cartRepository.GetByIdAsync(cartId);

        //        if (cart == null)
        //        {
        //            return ApiResponse<CheckoutResponseDto>.Failed(false, "Cart not found", 404, new List<string> { "Cart not found" });
        //        }

        //        string message = checkoutRequestDto.PaymentMethod switch
        //        {
        //            "Web" => "Payment processed via Web.",
        //            "USSD" => "Payment processed via USSD.",
        //            "Transfer" => "Payment processed via Transfer.",
        //            _ => "Invalid payment method."
        //        };

        //        if (message == "Invalid payment method.")
        //        {
        //            return ApiResponse<CheckoutResponseDto>.Failed(false, message, 400, new List<string> { message });
        //        }

        //        var checkout = new Checkout
        //        {
        //            CartId = cartId,
        //            AppUserId = cart.AppUserId, // Ensure this is set
        //            PaymentMethod = checkoutRequestDto.PaymentMethod,
        //            Status = "Success",
        //            CompletedAt = DateTime.UtcNow,
        //            Message = message
        //        };

        //        await _checkoutRepository.AddAsync(checkout);
        //        await _checkoutRepository.SaveChangesAsync();

        //        var purchaseHistory = new PurchaseHistory
        //        {
        //            AppUserId = cart.AppUserId,
        //            CheckoutId = checkout.Id,
        //            TotalAmount = cart.Books.Sum(b => b.Price), // Adjust according to your model
        //            PurchaseDate = DateTime.UtcNow
        //        };

        //        await _purchaseHistoryRepository.AddAsync(purchaseHistory);
        //        await _purchaseHistoryRepository.SaveChangesAsync();

        //        cart.Books?.Clear();
        //        _cartRepository.Update(cart);
        //        await _cartRepository.SaveChangesAsync();

        //        var checkoutResponse = _mapper.Map<CheckoutResponseDto>(checkout);

        //        return ApiResponse<CheckoutResponseDto>.Success(checkoutResponse, "Checkout completed successfully", 200);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred during checkout.");
        //        return ApiResponse<CheckoutResponseDto>.Failed(false, "An error occurred during checkout.", 500, new List<string> { ex.Message });
        //    }
        //}

        //public async Task<ApiResponse<CheckoutResponseDto>> CheckoutAsync(string cartId, CheckoutRequestDto checkoutRequestDto)
        //{
        //    try
        //    {
        //        var cart = await _cartRepository.GetByIdAsync(cartId);

        //        if (cart == null)
        //        {
        //            return ApiResponse<CheckoutResponseDto>.Failed(false, "Cart not found", 404, new List<string> { "Cart not found" });
        //        }

        //        string message = checkoutRequestDto.PaymentMethod switch
        //        {
        //            "Web" => "Payment processed via Web.",
        //            "USSD" => "Payment processed via USSD.",
        //            "Transfer" => "Payment processed via Transfer.",
        //            _ => "Invalid payment method."
        //        };

        //        if (message == "Invalid payment method.")
        //        {
        //            return ApiResponse<CheckoutResponseDto>.Failed(false, message, 400, new List<string> { message });
        //        }

        //        // Ensure the Books collection is initialized
        //        cart.Books = cart.Books ?? new List<Book>();

        //        var checkout = new Checkout
        //        {
        //            CartId = cartId,
        //            AppUserId = cart.AppUserId, // Ensure this is set
        //            PaymentMethod = checkoutRequestDto.PaymentMethod,
        //            Status = "Success",
        //            CompletedAt = DateTime.UtcNow,
        //            Message = message
        //        };

        //        await _checkoutRepository.AddAsync(checkout);
        //        await _checkoutRepository.SaveChangesAsync();

        //        // Check if there are any books in the cart before summing up their prices
        //        var totalAmount = cart.Books.Any() ? cart.Books.Sum(b => b.Price) : 0;

        //        var purchaseHistory = new PurchaseHistory
        //        {
        //            AppUserId = cart.AppUserId,
        //            CheckoutId = checkout.Id,
        //            TotalAmount = totalAmount,
        //            PurchaseDate = DateTime.Now
        //        };

        //        await _purchaseHistoryRepository.AddAsync(purchaseHistory);
        //        await _purchaseHistoryRepository.SaveChangesAsync();

        //        cart.Books.Clear();
        //        _cartRepository.Update(cart);
        //        await _cartRepository.SaveChangesAsync();

        //        var checkoutResponse = _mapper.Map<CheckoutResponseDto>(checkout);

        //        return ApiResponse<CheckoutResponseDto>.Success(checkoutResponse, "Checkout completed successfully", 200);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred during checkout.");
        //        return ApiResponse<CheckoutResponseDto>.Failed(false, "An error occurred during checkout.", 500, new List<string> { ex.Message });
        //    }
        //}

        // CheckoutAsync method
        public async Task<ApiResponse<CheckoutResponseDto>> CheckoutAsync(string cartId, CheckoutRequestDto checkoutRequestDto)
        {
            try
            {
                var cart = await _cartRepository.GetByIdAsync(cartId);

                if (cart == null)
                {
                    return ApiResponse<CheckoutResponseDto>.Failed(false, "Cart not found", 404, new List<string> { "Cart not found" });
                }

                string message = checkoutRequestDto.PaymentMethod switch
                {
                    "Web" => "Payment processed via Web.",
                    "USSD" => "Payment processed via USSD.",
                    "Transfer" => "Payment processed via Transfer.",
                    _ => "Invalid payment method."
                };

                if (message == "Invalid payment method.")
                {
                    return ApiResponse<CheckoutResponseDto>.Failed(false, message, 400, new List<string> { message });
                }

                cart.Books = cart.Books ?? new List<Book>();

                var checkout = new Checkout
                {
                    CartId = cartId,
                    AppUserId = cart.AppUserId,
                    PaymentMethod = checkoutRequestDto.PaymentMethod,
                    Status = "Success",
                    CompletedAt = DateTime.UtcNow,
                    Message = message
                };

                await _checkoutRepository.AddAsync(checkout);
                await _checkoutRepository.SaveChangesAsync();

                var totalAmount = cart.Books.Any() ? cart.Books.Sum(b => b.Price) : 0;

                var purchaseHistory = new PurchaseHistory
                {
                    AppUserId = cart.AppUserId,
                    CheckoutId = checkout.Id,
                    TotalAmount = totalAmount,
                    PurchaseDate = DateTime.Now,
                    Checkout = checkout
                };

                await _purchaseHistoryRepository.AddAsync(purchaseHistory);
                await _purchaseHistoryRepository.SaveChangesAsync();

                cart.Books.Clear();
                _cartRepository.Update(cart);
                await _cartRepository.SaveChangesAsync();

                var checkoutResponse = _mapper.Map<CheckoutResponseDto>(checkout);

                return ApiResponse<CheckoutResponseDto>.Success(checkoutResponse, "Checkout completed successfully", 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during checkout.");
                return ApiResponse<CheckoutResponseDto>.Failed(false, "An error occurred during checkout.", 500, new List<string> { ex.Message });
            }
        }
    }
}
