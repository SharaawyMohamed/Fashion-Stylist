using Microsoft.AspNetCore.Mvc;
using Web.Domain.Interfaces;

namespace Web.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMediaService _mediaService;

        public TestController(IMediaService mediaService)
        {
            this._mediaService = mediaService;
        }
        [HttpPost("UploadPhotoDeveloper")]
        public async Task<string> UploadPhotoDeveloper(IFormFile file)
        {
            return await _mediaService.UploadImageAsync(file, "DefaultPhoto");

        }

    }
}
