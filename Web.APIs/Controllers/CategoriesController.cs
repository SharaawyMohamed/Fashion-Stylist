using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Domain.Entites;
using Web.Infrastructure.Data;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CategoriesController(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var Categories = _dbContext.Set<Category>().Select(Cat => new { CatId = Cat.id, CatName = Cat.Name, CatPictureUrl = Cat.PictureUrl });
            return Ok(Categories);
        }
    }
}
