using System.Text.Json.Serialization;

namespace Web.Domain.DTOs.PaymentDTO
{
    public class PaymentDto
    {
        public decimal Amount { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
