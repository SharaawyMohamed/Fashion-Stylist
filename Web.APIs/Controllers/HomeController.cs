using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetHome(int? ProductType, int? CategoryId)
        {
            
            var AllProducts   = _context.Products;
            var BrandsProduct = AllProducts.ToList();
            var OffredProduct = AllProducts.Where(P => P.isOffred).ToList();

            if (ProductType is not null)
            {
                BrandsProduct = BrandsProduct.Where(P => P.ProductType == ProductType || P.ProductType == 2).ToList();
                OffredProduct = OffredProduct.Where(P => P.ProductType == ProductType || P.ProductType == 2).ToList();
            }

            if (CategoryId is not null)
            {
                BrandsProduct = BrandsProduct.Where(P => P.categoryId == CategoryId).ToList();
                OffredProduct = OffredProduct.Where(P => P.categoryId == CategoryId).ToList();
            }

            var GroubedBrandsProduct = BrandsProduct.GroupBy(p => p.brand).ToDictionary(g => g.Key, g => g.ToList());
            return Ok(new { OffredProdcut = OffredProduct, GroubedBrandsProduct });
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string ?Brand , int ?CategoryId , string? Qurey, int minPrice = 50 , int maxPrice = 10000)
        {
            var Products = _context.Products.Where(P => (Brand != null ? P.brand == Brand : true) && 
                                                     (CategoryId != null ? P.categoryId == CategoryId : true) &&
                                                     P.basePrice >= minPrice && P.basePrice <= maxPrice &&
                                                     (Qurey != null ? P.title.Contains(Qurey) : true)).ToList();

            return Ok(Products);

        }
    }
}
