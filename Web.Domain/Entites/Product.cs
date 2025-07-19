using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
	public class Product : BaseEntity<Guid>
	{
		public string title { get; set; }
		public string pictureUrl { get; set; }

		public decimal basePrice { get; set; }
		public decimal discountedPrice { get; set; }
		public float rate { get; set; }
		public string colors { get; set; }
		public string sizes { get; set; }
		public string brand { get; set; }
		public virtual ICollection<Review> reviews { get; set; }

		public int categoryId { get; set; }
		public virtual Category category { get; set; }

	}
}
