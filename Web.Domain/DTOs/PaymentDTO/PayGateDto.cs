namespace Web.Domain.DTOs.PaymentDTO
{
    public class PayGateDto
    {
        public string? PaymentGate { get; set; }
        public int? OrderId { get; set; }
        public decimal Amount { get; set; }
    }
}
