using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Enums;

namespace Web.Domain.Entites
{
	public class AppUser : IdentityUser
	{
		public string FullName { get; set; }
		public string FCM_Token { get; set; } = string.Empty;
		public string? ProfilePicture { get;set; } = string.Empty;
        public ICollection<Favorite> Favorites { get; set; } = new HashSet<Favorite>();
    }
}
