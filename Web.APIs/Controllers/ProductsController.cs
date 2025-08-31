using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web.Domain.DTOs.ProductDTO;
using Web.Domain.Entites;
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

        public ProductsController(AppDbContext dbContext, IMapper mapper)
        {
            _context = dbContext;
            this._mapper = mapper;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetProductById(Guid Id)
        {

            var product = await _context.Products
                                        .Include(p => p.reviews)
                                        .ThenInclude(r => r.appUser)
                                        .FirstOrDefaultAsync(p => p.id == Id);

            if (product == null)
                return NotFound("Product Not Found");

            var dto = _mapper.Map<ProductDto>(product);
            return Ok(dto);
        }

        [HttpPost("AddReview")]
        [Authorize]
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
            if (res == 0)
            {
                return BadRequest("Can't save Review");
            }
            var dto = await _context.Reviews
         .Include(u => u.appUser)
         .FirstOrDefaultAsync(p => p.id == rev.id);

            var response = new ReviewDto
            {
                Id = dto.id,
                Comment = dto.comment,
                Rate = dto.rate,
                Date = dto.CreatedAt,
                User = new UserDTO
                {
                    Id = dto.appUser.Id,
                    UserName = dto.appUser.FullName,
                    ProfilePicture = dto.appUser.ProfilePicture ?? "",
                    Email = dto.appUser.Email ?? "",

                }
            };
            return Ok(response);
        }

    }
}
