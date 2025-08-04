using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.DTOs.FavoritDTO
{
    public class FavoriteDto
    {
        public Guid Id {  get; set; }
        public string title { get; set; }
        public string pictureUrl { get; set; }
        public decimal basePrice { get; set; }
        public string colors { get; set; }
        public string sizes { get; set; }
    }
}
