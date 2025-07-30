using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
    public class Favorite
    {
        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
       public  Product Product { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
