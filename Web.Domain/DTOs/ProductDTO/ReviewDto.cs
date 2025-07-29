using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.DTOs.UserDTO;

namespace Web.Domain.DTOs.ProductDTO
{
    public class ReviewDto
    {
        public string Id { get; set; }
        public string Comment { get; set; }
        public double Rate { get; set; }
        public UserDto User { get; set; }
    }
}
