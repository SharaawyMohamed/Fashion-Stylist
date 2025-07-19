using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
	public class Item:BaseEntity<Guid>
	{
		public string title { get; set; }
		public string pictureUrl { get; set; }
		public string description { get; set; }

		public Guid CollectionId { get; set; }
		public virtual Collection collection { get; set; }
	}
}
