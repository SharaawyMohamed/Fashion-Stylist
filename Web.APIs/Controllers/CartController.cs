using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web.Domain.DTOs.CartDTO;
using Web.Domain.Entites;
using Web.Infrastructure.Data;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CartController(AppDbContext dbContext)
        {
            _context = dbContext;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User?.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = await _context.Carts
               .Include(c => c.Items)
               .ThenInclude(i => i.Product)
               .FirstOrDefaultAsync(c => c.UserAppId == userId);

            if (cart is null)
            {
                cart = new Cart
                {
                    UserAppId = userId
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }


            var cartDto = new GetCartDto
            {
                UserAppId = cart.UserAppId,
                Items = cart.Items.Select(item => new GetCartItemDto
                {
                    ProductId = item.ProductId,
                    ProductTitle = item.Product.title,
                    Quantity = item.Quantity,
                    Price = item.Product.basePrice-item.Product.discountedPrice,
                    Color = item.Color,
                    Size = item.sizes,

                    Image = item.Product.pictureUrl,
                    TotalPriceForProduct = item.TotalPriceForProduct
                }).ToList(),
                TotalAmount = cart.TotalAmount
            };
            return Ok(cartDto);
        }
        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> AddToCart([FromBody] SendCartItemDTO sendCartItemDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserAppId == userId)
                     ?? new Cart { UserAppId = userId };

            if (cart.id == 0)
            {
                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();
            }

            var existingItem = await _context.CartItems.FirstOrDefaultAsync(i => i.CartId == cart.id && i.ProductId == sendCartItemDTO.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += sendCartItemDTO.Quantity;
                await _context.SaveChangesAsync();
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductId = sendCartItemDTO.ProductId,
                    Quantity = sendCartItemDTO.Quantity,
                    CartId = cart.id,
                    sizes = sendCartItemDTO.Size,
                    Color = sendCartItemDTO.Color

                };
                await _context.CartItems.AddAsync(cartItem);
            }

            await _context.SaveChangesAsync();
            return Ok("Product Added ");
        }
        [Authorize]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromCart(Guid productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserAppId == userId);
            if (cart == null) return Ok("Cart not found.");

            var item = await _context.CartItems.FirstOrDefaultAsync(i => i.CartId == cart.id && i.ProductId == productId);
            if (item == null) return Ok("Item not found in cart.");

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return Ok("Item was Deleted");
        }
        [Authorize]
        [HttpPost("increment/{productId}")]
        public async Task<IActionResult> IncrementItemQuantity(Guid productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.Carts.Include(c => c.Items)
       .ThenInclude(i => i.Product)
       .FirstOrDefaultAsync(c => c.UserAppId == userId);
            if (cart == null) return Ok("Cart not found.");
            var item = await _context.CartItems.FirstOrDefaultAsync(i => i.CartId == cart.id && i.ProductId == productId);
            if (item == null) return Ok("Item not found in cart.");

            item.Quantity++;

            await _context.SaveChangesAsync();
            return Ok(new
            {
                item.ProductId,
                item.Quantity,
                item.TotalPriceForProduct
            });

        }

        [HttpPost("decrement/{productId}")]
        public async Task<IActionResult> DecrementItemQuantity(Guid productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.Carts.Include(c => c.Items)
       .ThenInclude(i => i.Product)
       .FirstOrDefaultAsync(c => c.UserAppId == userId);
            if (cart == null) return Ok("Cart not found.");
            var item = await _context.CartItems.FirstOrDefaultAsync(i => i.CartId == cart.id && i.ProductId == productId);
            if (item == null) return Ok("Item not found in cart.");

            item.Quantity--;
            item.Quantity = Math.Max(item.Quantity, 0);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                item.ProductId,
                item.Quantity,
                item.TotalPriceForProduct
            });
        }
    }
}
