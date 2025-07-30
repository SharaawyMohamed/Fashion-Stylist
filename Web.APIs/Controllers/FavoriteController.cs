using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web.Domain.Entites;
using Web.Domain.Response;
using Web.Infrastructure.Data;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public FavoriteController(AppDbContext context)
        {
            _dbContext = context;
        }
        [Authorize]
        [HttpPost("{ProductId}")]
        public async Task<IActionResult> GetAllFavoriteByUserId(Guid ProductId)

        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.id == ProductId);
            if (product == null)
            {
             return BadRequest("No Product with this id");
            }

            var isFavorit = await _dbContext.Favorites.FirstOrDefaultAsync(f => f.UserId == UserId && f.ProductId == ProductId);
            if (isFavorit != null)
            {
                _dbContext.Remove(isFavorit);
                await _dbContext.SaveChangesAsync();
                return Ok("The product added to your favorite");

            }
            var Favorit = new Favorite { ProductId = ProductId, UserId = UserId };
            await _dbContext.AddAsync(Favorit);
            await _dbContext.SaveChangesAsync();

            return Ok("The product Deleted from your favorite");

        }

    }
}
