using CartMicroservice.Data;
using CartMicroservice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CartMicroservice.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly CartContext _context;

        public CartController(CartContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDto cartItemDto)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == cartItemDto.ProductId && c.UserId == cartItemDto.UserId);

            if (cartItem != null)
            {
                cartItem.Quantity += cartItemDto.Quantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    ProductId = cartItemDto.ProductId,
                    UserId = cartItemDto.UserId,
                    Quantity = cartItemDto.Quantity
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Item added to cart successfully" });
        }

        [HttpGet("items")]
        [Authorize]
        public async Task<IActionResult> GetCartItems([FromQuery] string userId)
        {
            var cartItems = _context.CartItems.Where(c => c.UserId == userId).ToList();

            if (cartItems.Count == 0)
            {
                return NotFound(new { Message = "No items found in cart" });
            }

            return Ok(cartItems);
        }

        [HttpPost("increment")]
        [Authorize]
        public async Task<IActionResult> IncrementCartItem([FromBody] CartItemDto cartItemDto)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == cartItemDto.ProductId && c.UserId == cartItemDto.UserId);

            if (cartItem == null)
            {
                return NotFound(new { Message = "Item not found in cart" });
            }

            cartItem.Quantity += 1;
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Quantity incremented successfully" });
        }

        [HttpDelete("remove-completely")]
        [Authorize]
        public async Task<IActionResult> RemoveCompletely([FromBody] CartItemDto cartItemDto)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == cartItemDto.ProductId && c.UserId == cartItemDto.UserId);

            if (cartItem == null)
            {
                return NotFound(new { Message = "Item not found in cart" });
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Item removed from cart completely" });
        }
        [HttpDelete("remove")]
        [Authorize]
        public async Task<IActionResult> DecrementCartItem([FromBody] CartItemDto cartItemDto)
        {
            if (cartItemDto == null || string.IsNullOrEmpty(cartItemDto.ProductId) || string.IsNullOrEmpty(cartItemDto.UserId))
            {
                return BadRequest(new { Message = "Invalid request data." });
            }

            // Fetch the cart item for the given user and product
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == cartItemDto.ProductId && c.UserId == cartItemDto.UserId);

            if (cartItem == null)
            {
                return NotFound(new { Message = "Item not found in the cart." });
            }

            // Decrement the quantity
            cartItem.Quantity -= cartItemDto.Quantity;

            if (cartItem.Quantity <= 0)
            {
                // If quantity becomes 0 or less, remove the item from the cart
                _context.CartItems.Remove(cartItem);
            }

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Quantity decremented successfully." });
        }



    }

}
