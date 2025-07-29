using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
	public class Collection:BaseEntity<Guid>
	{
		public string title { get; set; }
		public string subTitle { get; set; }
		public string description { get; set; }
		public decimal price { get; set; }
		public string pictureUrl { get; set; }
		public virtual ICollection<Item> items { get; set; } = new List<Item>();
	}
	
}
