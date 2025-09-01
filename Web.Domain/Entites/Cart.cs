namespace Web.Domain.Entites
{
    public class Cart : BaseEntity<int>
    {

        public string UserAppId { get; set; }
        public AppUser UserApp { get; set; }
        public decimal TotalAmount => CalculateTotal();
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        private decimal CalculateTotal()
        {
            return Items.Sum(item => (item.Product.basePrice - item.Product.discountedPrice) * item.Quantity);
        }
    }
}


