using BookBarn.Application.DTOs.Cart;
using BookBarn.Application.DTOs.Checkout;
using BookBarn.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookBarn.Controllers
{
    [Route("api/cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] CreateCartDto createCartDto)
        {
            var response = await _cartService.AddCartAsync(createCartDto);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("{cartId}/books/{bookId}")]
        public async Task<IActionResult> AddBookToCart(string cartId, string bookId)
        {
            var response = await _cartService.AddBookToCartAsync(cartId, bookId);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("{cartId}")]
        public async Task<IActionResult> DeleteCart(string cartId)
        {
            var response = await _cartService.DeleteCartAsync(cartId);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCarts()
        {
            var response = await _cartService.GetAllCartsAsync();
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCartById(string cartId)
        {
            var response = await _cartService.GetCartByIdAsync(cartId);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("{cartId}/books/{bookId}")]
        public async Task<IActionResult> RemoveBookFromCart(string cartId, string bookId)
        {
            var response = await _cartService.RemoveBookFromCartAsync(cartId, bookId);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCart([FromBody] UpdateCartDto updateCartDto)
        {
            var response = await _cartService.UpdateCartAsync(updateCartDto);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("checkout/{cartId}")]
        public async Task<IActionResult> Checkout(string cartId, [FromBody] CheckoutRequestDto checkoutRequestDto)
        {
            var response = await _cartService.CheckoutAsync(cartId, checkoutRequestDto);
            if (response.Succeeded)
                return Ok(response);
            return BadRequest(response);
        }

    }
}
