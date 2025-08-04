using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.DTOs.CartDTO
{
    public class GetCartDto
    {
        public string UserAppId { get; set; }
        public List<GetCartItemDto> Items { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
