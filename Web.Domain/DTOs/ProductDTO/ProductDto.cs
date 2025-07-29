using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.DTOs.ProductDTO
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string PictureUrl { get; set; }
        public decimal BasePrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public float Rate { get; set; }
        public string Colors { get; set; }     
        public string Sizes { get; set; }      
        public string Brand { get; set; }
        public bool IsOffred { get; set; }
        public int ProductType { get; set; }
        public int CategoryId { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }
}
