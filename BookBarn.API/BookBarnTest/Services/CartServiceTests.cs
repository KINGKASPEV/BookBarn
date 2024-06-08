using AutoMapper;
using BookBarn.Application.DTOs.Book;
using BookBarn.Application.DTOs.Cart;
using BookBarn.Application.Services.Implementations;
using BookBarn.Domain.Entities;
using BookBarn.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace BookBarnTest.Services
{
    public class CartServiceTests
    {
        private readonly Mock<IGenericRepository<Cart>> _mockCartRepository;
        private readonly Mock<IGenericRepository<Book>> _mockBookRepository;
        private readonly Mock<IGenericRepository<Checkout>> _mockCheckoutRepository;
        private readonly Mock<IGenericRepository<PurchaseHistory>> _mockPurchaseHistoryRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<CartService>> _mockLogger;
        private readonly CartService _cartService;

        public CartServiceTests()
        {
            _mockCartRepository = new Mock<IGenericRepository<Cart>>();
            _mockBookRepository = new Mock<IGenericRepository<Book>>();
            _mockCheckoutRepository = new Mock<IGenericRepository<Checkout>>();
            _mockPurchaseHistoryRepository = new Mock<IGenericRepository<PurchaseHistory>>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<CartService>>();
            _cartService = new CartService(
                _mockCartRepository.Object,
                _mockBookRepository.Object,
                _mockCheckoutRepository.Object,
                _mockPurchaseHistoryRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task AddCartAsync_ShouldReturnSuccess_WhenCartIsCreated()
        {
            // Arrange
            var createCartDto = new CreateCartDto { BookIds = new List<string> { "bookId1", "bookId2" } };
            var cart = new Cart { Id = Guid.NewGuid().ToString(), Books = new List<Book>() };
            var book = new Book { Id = "bookId1", Title = "Test Book" };

            _mockMapper.Setup(m => m.Map<Cart>(createCartDto)).Returns(cart);
            _mockBookRepository.Setup(r => r.GetByIdAsync("bookId1")).ReturnsAsync(book);
            _mockBookRepository.Setup(r => r.GetByIdAsync("bookId2")).ReturnsAsync((Book)null);
            _mockCartRepository.Setup(r => r.AddAsync(cart)).Returns(Task.CompletedTask);
            _mockCartRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<CartDto>(cart)).Returns(new CartDto { Id = cart.Id, Books = new List<BookDto>() });

            // Act
            var result = await _cartService.AddCartAsync(createCartDto);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Cart created successfully", result.Message);
        }

        [Fact]
        public async Task AddBookToCartAsync_ShouldReturnSuccess_WhenBookIsAdded()
        {
            // Arrange
            var cartId = "cartId";
            var bookId = "bookId";
            var cart = new Cart { Id = cartId, Books = new List<Book>() };
            var book = new Book { Id = bookId, Title = "Test Book" };

            _mockCartRepository.Setup(r => r.GetByIdAsync(cartId)).ReturnsAsync(cart);
            _mockBookRepository.Setup(r => r.GetByIdAsync(bookId)).ReturnsAsync(book);
            _mockCartRepository.Setup(r => r.Update(cart));
            _mockCartRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<CartDto>(cart)).Returns(new CartDto { Id = cart.Id, Books = new List<BookDto>() });

            // Act
            var result = await _cartService.AddBookToCartAsync(cartId, bookId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Book added to cart successfully", result.Message);
        }

        [Fact]
        public async Task DeleteCartAsync_ShouldReturnSuccess_WhenCartIsDeleted()
        {
            // Arrange
            var cartId = "cartId";
            var cart = new Cart { Id = cartId };

            _mockCartRepository.Setup(r => r.GetByIdAsync(cartId)).ReturnsAsync(cart);
            _mockCartRepository.Setup(r => r.Delete(cart));
            _mockCartRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _cartService.DeleteCartAsync(cartId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Cart deleted successfully", result.Message);
        }

        [Fact]
        public async Task GetAllCartsAsync_ShouldReturnSuccess_WhenCartsAreRetrieved()
        {
            // Arrange
            var carts = new List<Cart> { new Cart { Id = "cartId1" }, new Cart { Id = "cartId2" } };

            _mockCartRepository.Setup(r => r.GetAllToIncludeAsync(null)).ReturnsAsync(carts);
            _mockMapper.Setup(m => m.Map<IEnumerable<CartDto>>(carts)).Returns(new List<CartDto> { new CartDto { Id = "cartId1" }, new CartDto { Id = "cartId2" } });

            // Act
            var result = await _cartService.GetAllCartsAsync();

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Carts retrieved successfully", result.Message);
        }


        [Fact]
        public async Task GetCartByIdAsync_ShouldReturnSuccess_WhenCartIsRetrieved()
        {
            // Arrange
            var cartId = "cartId";
            var cart = new Cart { Id = cartId };

            _mockCartRepository.Setup(r => r.GetByIdAsync(cartId)).ReturnsAsync(cart);
            _mockMapper.Setup(m => m.Map<CartDto>(cart)).Returns(new CartDto { Id = cartId });

            // Act
            var result = await _cartService.GetCartByIdAsync(cartId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Cart retrieved successfully", result.Message);
        }

        [Fact]
        public async Task RemoveBookFromCartAsync_ShouldReturnSuccess_WhenBookIsRemoved()
        {
            // Arrange
            var cartId = "cartId";
            var bookId = "bookId";
            var cart = new Cart { Id = cartId, Books = new List<Book> { new Book { Id = bookId, Title = "Test Book" } } };

            _mockCartRepository.Setup(r => r.GetByIdAsync(cartId)).ReturnsAsync(cart);
            _mockCartRepository.Setup(r => r.Update(cart));
            _mockCartRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<CartDto>(cart)).Returns(new CartDto { Id = cartId });

            // Act
            var result = await _cartService.RemoveBookFromCartAsync(cartId, bookId);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Book removed from cart successfully", result.Message);
        }

        [Fact]
        public async Task UpdateCartAsync_ShouldReturnSuccess_WhenCartIsUpdated()
        {
            // Arrange
            var updateCartDto = new UpdateCartDto { CartId = "cartId", BookIds = new List<string> { "bookId1", "bookId2" } };
            var cart = new Cart { Id = "cartId", Books = new List<Book>() };
            var book1 = new Book { Id = "bookId1", Title = "Test Book 1" };
            var book2 = new Book { Id = "bookId2", Title = "Test Book 2" };

            _mockCartRepository.Setup(r => r.GetByIdAsync("cartId")).ReturnsAsync(cart);
            _mockBookRepository.Setup(r => r.GetByIdAsync("bookId1")).ReturnsAsync(book1);
            _mockBookRepository.Setup(r => r.GetByIdAsync("bookId2")).ReturnsAsync(book2);
            _mockCartRepository.Setup(r => r.Update(cart));
            _mockCartRepository.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<CartDto>(cart)).Returns(new CartDto { Id = cart.Id, Books = new List<BookDto> { new BookDto { Id = "bookId1" }, new BookDto { Id = "bookId2" } } });

            // Act
            var result = await _cartService.UpdateCartAsync(updateCartDto);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Cart updated successfully", result.Message);
        }
    }
}
