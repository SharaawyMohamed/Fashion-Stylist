using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using Web.Application.Utility;
using Web.Domain.DTOs.FavoritDTO;
using Web.Domain.DTOs.ProductDTO;
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
        private readonly IMapper _mapper;
        public FavoriteController(AppDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }
        [Authorize]
        [HttpPost("{ProductId}")]
        public async Task<IActionResult> AddOrRemoveToFavoriteByProductId(Guid ProductId)

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
                return Ok("The product  Deleted to your favorite");

            }
            var Favorit = new Favorite { ProductId = ProductId, UserId = UserId };
            await _dbContext.AddAsync(Favorit);
            await _dbContext.SaveChangesAsync();

            return Ok("The product added from your favorite");

        }
        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetAllFavorites()

        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;         

            var favorits = await _dbContext.Favorites.Where(f => f.UserId == UserId).ToListAsync();
            if (favorits == null || !favorits.Any())           
                return Ok("You Don't have product in Your favorite");
            
            var response = new List<FavoriteDto>();
            foreach (var i in favorits)
            {
                var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.id == i.ProductId);
                if (product != null)
                {
                    var dto = new FavoriteDto
                    {
                        Id = i.ProductId,
                        title = product.title,
                        basePrice = product.basePrice,
                        pictureUrl = product.pictureUrl
                    };
                    response.Add(dto);
                }
            }
           

            return Ok(response);
        }

        }
}
