using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Web.Domain.DTOs.PaymentDTO;

namespace Web.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymobController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public PaymobController(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        // 1️⃣ Auth Token
        private async Task<string> GetAuthTokenAsync()
        {
            var response = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/auth/tokens",
                new { api_key = _config["Paymob:ApiKey"] });

            var result = await response.Content.ReadFromJsonAsync<AuthTokenResponse>();
            return result.Token;
        }

        // 2️⃣ Create Order
        private async Task<int> CreateOrderAsync(string token, decimal amount)
        {
            var response = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/ecommerce/orders", new
            {
                auth_token = token,
                delivery_needed = false,
                amount_cents = (int)(amount * 100),
                currency = "EGP",
                items = new object[] { }
            });

            var result = await response.Content.ReadFromJsonAsync<CreateOrderResponse>();
            return result.Id;
        }

        // 3️⃣ Payment Key
        private async Task<string> GetPaymentKeyAsync(string token, int orderId, decimal amount, string First_name, string Last_name, string Email, string PhoneNumber)
        {
            var response = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/acceptance/payment_keys", new
            {
                auth_token = token,
                amount_cents = (int)(amount * 100),
                expiration = 3600,
                order_id = orderId,
                billing_data = new
                {
                    apartment = "NA",
                    email = Email,
                    floor = "NA",
                    first_name = First_name,
                    street = "NA",
                    building = "NA",
                    phone_number = PhoneNumber,
                    shipping_method = "NA",
                    postal_code = "NA",
                    city = "Cairo",
                    country = "EG",
                    last_name = Last_name,
                    state = "NA"
                },

                currency = "EGP",
                integration_id = int.Parse(_config["Paymob:IntegrationId"])
            });

            var result = await response.Content.ReadFromJsonAsync<PaymentKeyResponse>();
            return result.Token;
        }

        
        [HttpPost("pay")]
        public async Task<IActionResult> Pay([FromBody] PaymentDto dto)
        {
            var token = await GetAuthTokenAsync();
            var orderId = await CreateOrderAsync(token, dto.Amount);
            var paymentKey = await GetPaymentKeyAsync(token, orderId, dto.Amount, dto.FName, dto.LName, dto.Email, dto.PhoneNumber);


            string iframeUrl = $"https://accept.paymob.com/api/acceptance/iframes/{_config["Paymob:IframeId"]}?payment_token={paymentKey}";

            return Ok(new { url = iframeUrl });
        }


        [HttpPost("webhook")]
        public IActionResult PaymobWebhook([FromBody] object payload)
        {
            // هنا لازم تتحقق من HMAC وتعمل Update للـ DB حسب النجاح أو الفشل
            return Ok();
        }
    }

    // DTOs
    public class PayDto
    {
        public decimal Amount { get; set; }
        public string UserName { get; set; }
    }

    public class AuthTokenResponse
    {
        public string Token { get; set; }
    }

    public class CreateOrderResponse
    {
        public int Id { get; set; }
    }

    public class PaymentKeyResponse
    {
        public string Token { get; set; }
    }
}
