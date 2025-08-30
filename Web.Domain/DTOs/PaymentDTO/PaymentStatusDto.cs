namespace Web.Domain.DTOs.PaymentDTO
{
    public class PaymentStatusDto
    {
        public bool pending { get; set; }
        public bool success { get; set; }
        public DateTime TransTime { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string? DeliveryDate { get; set; }
        public decimal OrderPrice { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}
