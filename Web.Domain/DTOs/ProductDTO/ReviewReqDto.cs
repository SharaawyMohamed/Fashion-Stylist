using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.DTOs.ProductDTO
{
    public class ReviewReqDto
    {
        public string Comment { set; get; }
        public double rate { set; get; }
        public Guid ProductId { set; get; }
    }
}
