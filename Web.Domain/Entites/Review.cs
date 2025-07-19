using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
	public class Review:BaseEntity<Guid>
	{
		public string comment { get; set; }
		public int rate { get; set; }

		public string userId { get; set; }
		public virtual AppUser appUser { get; set; }

		public Guid productId { get; set; }
		public virtual Product product { get; set; }
	}
}
