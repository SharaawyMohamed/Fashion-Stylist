using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Entites;

namespace Web.Domain.Interfaces
{
	public interface IAuthService
	{
		Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> _userManager);

	}
}
