using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Domain.DTOs.PaymentDTO;
using Web.Domain.Entites;
using Web.Domain.Interfaces;

namespace Web.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymobController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IPaymopService _paymopService;
        private readonly UserManager<AppUser> _userManager;

        public PaymobController(HttpClient httpClient, IConfiguration config, IPaymopService paymopService, UserManager<AppUser> userManager)
        {
            _httpClient = httpClient;
            _config = config;
            this._paymopService = paymopService;
            this._userManager = userManager;
        }

        [Authorize]
        [HttpPost("pay")]
        public async Task<ActionResult<PayGateDto>> Pay([FromBody] PaymentDto Model)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            Model.UserId = (await _userManager.FindByEmailAsync(Email)).Id;
            return Ok(await _paymopService.GetPayGateAsync(Model));
        }


        [Authorize]
        [HttpPost("GetPaymentStatus")]
        public async Task<ActionResult<PaymentStatusDto>> GetPaymentStatus([FromQuery] int OrderId)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await _paymopService.GetPaymentStatusAsync(OrderId, UserId ?? ""));
        }

    }
}
