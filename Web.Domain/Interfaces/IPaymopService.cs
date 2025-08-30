using Web.Domain.DTOs.PaymentDTO;

namespace Web.Domain.Interfaces
{
    public interface IPaymopService
    {
        public Task<string?> GetAuthTokenAsync();
        public Task<int?> CreateOrderAsync(string token, decimal amount);
        public Task<string> GetPaymentKeyAsync(string token, int orderId, decimal amount, string First_name, string Last_name, string Email, string PhoneNumber);

        public Task<PayGateDto> GetPayGateAsync(PaymentDto Model);
        public Task<PaymentStatusDto> GetPaymentStatusAsync(int OrderId, string UserId);

    }
}
