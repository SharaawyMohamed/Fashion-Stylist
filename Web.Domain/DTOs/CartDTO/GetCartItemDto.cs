using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.DTOs.CartDTO
{
    public class GetCartItemDto
    {
        public Guid ProductId { get; set; }
        public string ProductTitle { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
      
        public string Image { get; set; }

    
        public string Size { get; set; }

      
        public string Color { get; set; }
      
        public decimal TotalPriceForProduct { get; set; }
    }
}
