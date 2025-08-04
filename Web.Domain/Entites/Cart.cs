using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
    public class Cart:BaseEntity<int>
    {
            
        public string UserAppId { get; set; }
        public AppUser UserApp { get; set; }
        public decimal TotalAmount => CalculateTotal();
    public List<CartItem> Items { get; set; } = new List<CartItem>();
        private decimal CalculateTotal()
        {
            return Items.Sum(item => item.Product.basePrice * item.Quantity);
        }
    }
}


