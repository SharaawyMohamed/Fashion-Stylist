using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Domain.Entites;
using Web.Infrastructure.Data;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetHome(int? ProductType, int? CategoryId , int pageIndex = 1, int pageSize = 20)
        {
          
            var query = _context.Products.AsQueryable();

           
            if (ProductType is not null)
            {
                query = query.Where(P => P.ProductType == ProductType || P.ProductType == 2);
            }

            if (CategoryId is not null)
            {
                query = query.Where(P => P.categoryId == CategoryId);
            }

            
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

          
            var products = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            
            var groupedBrandsProduct = products
                .GroupBy(p => p.brand)
                .ToDictionary(g => g.Key, g => g.ToList());

            return Ok(new
            {
                GroubedBrandsProduct = groupedBrandsProduct,
                TotalPages = totalPages,
            });
        }

        [HttpGet("OfferedProducts")]
        public async Task<IActionResult> GetOfferedProducts(int? ProductType, int? CategoryId , int pageIndex = 1 , int pageSize = 20)
        {
          
            var query = _context.Products
                .Where(P => P.isOffred &&
                           (ProductType != null ? P.ProductType == ProductType : true) &&
                           (CategoryId != null ? P.categoryId == CategoryId : true));

           
            var totalItems = await query.CountAsync();

            
            var offredProducts = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return Ok(new
            {
                data = offredProducts,
                totalPages = totalPages
            });
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string ?Brand , int ?CategoryId , string? Qurey, int minPrice = 50 , int maxPrice = 10000 , int pageSize = 20 , int pageIndex = 1)
        {
            var query = _context.Products.AsQueryable();

            
            if (Brand != null)
                query = query.Where(P => P.brand == Brand);

            if (CategoryId != null)
                query = query.Where(P => P.categoryId == CategoryId);

            query = query.Where(P => P.basePrice >= minPrice && P.basePrice <= maxPrice);

            if (Qurey != null)
                query = query.Where(P => P.title.Contains(Qurey));

          
            var totalItems = await query.CountAsync();

            
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

           
            var products = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                Data = products,
                TotalPages = totalPages            
            });

        }
    }
}
