using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Web.Domain.DTOs.ProductDTO;
using Web.Domain.DTOs.UserDTO;
using Web.Domain.Entites;
using Web.Domain.Repositories;
using Web.Infrastructure.Data;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(AppDbContext dbContext , IMapper mapper)
        {
           _context = dbContext;
            this._mapper = mapper;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetProductById(Guid Id) {

            var product = await _context.Products
                                        .Include(p => p.reviews)
                                        .ThenInclude(r => r.appUser)
                                        .FirstOrDefaultAsync(p => p.id == Id);

            if (product == null)
                return NotFound("Product Not Found");

            var dto =  _mapper.Map<ProductDto>(product);
            return Ok(dto);
        }

        [HttpPost("AddReview")]
        public async Task<IActionResult> AddReview(ReviewReqDto review)
        {
            Review rev = new Review()
            {
                comment = review.Comment,
                rate = review.rate,
                productId = review.ProductId,
                userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value
            };
            _context.Reviews.Add(rev);
            int res = _context.SaveChanges();
            if(res == 0)
            {
                return BadRequest("Can't save Review");
            }
            return Ok("Review added");
        }

    }
}
