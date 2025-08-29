using Microsoft.AspNetCore.Mvc;
using Web.Domain.DTOs.PaymentDTO;
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

        public PaymobController(HttpClient httpClient, IConfiguration config, IPaymopService paymopService)
        {
            _httpClient = httpClient;
            _config = config;
            this._paymopService = paymopService;
        }

        [HttpPost("pay")]
        public async Task<ActionResult<PayGateDto>> Pay([FromBody] PaymentDto Model)
        {
            return Ok(await _paymopService.GetPayGateAsync(Model));
        }


        [HttpPost("GetPaymentStatus")]
        public async Task<ActionResult<PaymentStatusDto>> GetPaymentStatus([FromQuery] int OrderId)
        {
            return Ok(await _paymopService.GetPaymentStatusAsync(OrderId));
        }

    }
}
