using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Domain.Entites
{
    public class CartItem
    {
        [ForeignKey(nameof(Cart))]
        public int CartId { get; set; }
        public Cart Cart { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public string sizes { get; set; }
        public string Color { get; set; }
        public decimal TotalPriceForProduct => CalculateTotal();

        private decimal CalculateTotal()
        {
            return (Product.basePrice - (Product.isOffred ? Product.discountedPrice : 0m)) * Quantity;
        }
    }
}
