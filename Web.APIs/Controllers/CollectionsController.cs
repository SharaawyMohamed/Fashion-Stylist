using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Infrastructure.Data;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CollectionsController(AppDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCollections()
        {
            return Ok(_dbContext.collections.ToList());
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCollectionDetails(Guid id)
        {
            var CollectionDetails = _dbContext.collections.Include(Col => Col.items).FirstOrDefault(Col => Col.id == id);

            if(CollectionDetails == null)
            {
                return BadRequest("Collection not Exist any more ");
            }else
                return Ok(CollectionDetails);
        }

        [HttpGet("Item/{id}")]
        public async Task<IActionResult> GetCollectionItemDetails(Guid id)
        {
            var ItemDetails = _dbContext.Products.FirstOrDefault(p => p.id == id);
            if(ItemDetails == null)
            {
                return BadRequest("This Item not Exist any more");
            }else
                return Ok(ItemDetails);
        }
    }
}
