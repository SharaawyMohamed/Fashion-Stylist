using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;
using Web.Domain.DTOs.PaymentDTO;
using Web.Domain.Interfaces;
using Web.Infrastructure.Data;

namespace Web.Application.Service
{
    public class PaymopService : IPaymopService
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IAccountService _accountService;
        private readonly AppDbContext _appDbContext;

        public PaymopService(HttpClient httpClient
            , IConfiguration config
            , IAccountService accountService
            , AppDbContext appDbContext)
        {
            _httpClient = httpClient;
            _config = config;
            _accountService = accountService;
            _appDbContext = appDbContext;
        }

        public async Task<string?> GetAuthTokenAsync()
        {
            var response = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/auth/tokens",
            new { api_key = _config["Paymob:ApiKey"] });

            var JsonDoc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var token = JsonDoc.RootElement.GetProperty("token").GetString();
            return token;
        }


        public async Task<int?> CreateOrderAsync(string token, decimal amount)
        {
            var response = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/ecommerce/orders", new
            {
                auth_token = token,
                delivery_needed = false,
                amount_cents = (int)(amount * 100),
                currency = "EGP",
                items = new object[] { }
            });

            var JsonDoc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var id = JsonDoc.RootElement.GetProperty("id").GetInt32();
            return id;
        }


        public async Task<string> GetPaymentKeyAsync(string token, int orderId, decimal amount, string First_name, string Last_name, string Email, string PhoneNumber)
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

            var JsonDoc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var paymentKey = JsonDoc?.RootElement.GetProperty("token").GetString() ?? "";
            return paymentKey;
        }

        public async Task<PayGateDto> GetPayGateAsync(PaymentDto Model)
        {
            var token = await GetAuthTokenAsync();
            var orderId = await CreateOrderAsync(token ?? "", Model.Amount);
            var paymentKey = await GetPaymentKeyAsync(token, orderId.Value, Model.Amount, Model.FName, Model.LName, Model.Email, Model.PhoneNumber);


            string iframeUrl = $"https://accept.paymob.com/api/acceptance/iframes/{_config["Paymob:IframeId"]}?payment_token={paymentKey}";

            await _accountService.AddTransAsync(Model.UserId, orderId.Value, Model.Amount);

            return new PayGateDto
            {
                PaymentGate = iframeUrl,
                OrderId = orderId ?? 0,
                Amount = Model.Amount
            };
        }

        public async Task<PaymentStatusDto> GetPaymentStatusAsync(int OrderId, string UserId)
        {
            var token = await GetAuthTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync("https://accept.paymob.com/api/ecommerce/orders/transaction_inquiry",
                new
                {
                    order_id = OrderId
                });
            if (response.IsSuccessStatusCode == false)
            {
                return new PaymentStatusDto
                {
                    pending = false,
                    success = false,
                    Date = "NA",
                    Time = "NA",
                    FName = "NA",
                    LName = "NA",
                    OrderPrice = 0,
                    TransTime = DateTime.MinValue
                };
            }
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var root = doc.RootElement;
            var StatusData = new PaymentStatusDto
            {
                pending = root.GetProperty("pending").GetBoolean(),
                success = root.GetProperty("success").GetBoolean(),
                TransTime = root.GetProperty("created_at").GetDateTime(),
                OrderPrice = root.GetProperty("amount_cents").GetDecimal() / 100,
                FName = root.GetProperty("order").GetProperty("shipping_data").GetProperty("first_name").GetString() ?? "NA",
                LName = root.GetProperty("order").GetProperty("shipping_data").GetProperty("last_name").GetString() ?? "NA"
            };
            StatusData.Date = StatusData.TransTime.ToString("dd-MMM-yyyy");
            StatusData.Time = StatusData.TransTime.ToString("hh:mm tt");
            StatusData.DeliveryDate = StatusData.TransTime.AddDays(2).ToString("dd-MMM-yyyy");

            var LstOrder = await _appDbContext.Users.Where(AppContext => AppContext.Id == UserId).Include(U => U.transactions).Select(U => U.transactions.Last()).FirstOrDefaultAsync();

            if (LstOrder?.TransactionId == OrderId)
            {
                var UserCart = _appDbContext.Carts.Where(C => C.UserAppId == UserId).FirstOrDefault();
                if (UserCart is not null)
                    _appDbContext.Carts.Remove(UserCart!);
                await _appDbContext.SaveChangesAsync();
            }

            return StatusData;
        }
    }
}
