using System.Text.Json.Serialization;

namespace Web.Domain.DTOs.PaymentDTO
{
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
        public string? DeliveryDate { get; set; }
        public decimal OrderPrice { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}
