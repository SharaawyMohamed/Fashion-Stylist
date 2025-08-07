using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Web.Domain.Entites
{
	public class Review:BaseEntity<Guid>
	{
		public string comment { get; set; }
		public double rate { get; set; }
		public DateOnly CreatedAt {  get; set; } = DateOnly.FromDateTime(DateTime.Now); 
		public string userId { get; set; }
		public virtual AppUser appUser { get; set; }

		public Guid productId { get; set; }
		[JsonIgnore]
		public virtual Product product { get; set; }
	}
}
