using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
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

        [HttpPost("GetPaymentStatus")]
        public async Task<ActionResult<PaymentStatusDto>> GetPaymentStatus([FromQuery] int OrderId)
        {

            _httpClient.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetAuthTokenAsync());

            var response = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/ecommerce/orders/transaction_inquiry",
                new
                {
                    order_id = OrderId
                });
            //var result = await response.Content.ReadFromJsonAsync<PaymentStatusDto>();
            //result.OrderPrice /= 100;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var root = doc.RootElement;
            var status = new PaymentStatusDto
            {
                pending = root.GetProperty("pending").GetBoolean(),
                success = root.GetProperty("success").GetBoolean(),
                TransTime = root.GetProperty("created_at").GetDateTime(),
                OrderPrice = root.GetProperty("amount_cents").GetDecimal() / 100,
                FName = root.GetProperty("order").GetProperty("shipping_data").GetProperty("first_name").GetString() ?? "NA",
                LName = root.GetProperty("order").GetProperty("shipping_data").GetProperty("last_name").GetString() ?? "NA"
            };

            return Ok(status);
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

    public class PaymentStatusDto
    {
        public bool pending { get; set; }
        public bool success { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime TransTime { get; set; }
        [JsonPropertyName("first_name")]
        public string FName { get; set; }
        [JsonPropertyName("last_name")]
        public string LName { get; set; }
        [JsonPropertyName("amount_cents")]
        public decimal OrderPrice { get; set; }
    }


}
