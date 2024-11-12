using CartMicroservice.Data;
using CartMicroservice.Models;
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
    }

}
