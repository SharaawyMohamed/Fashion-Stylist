using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.DTOs.AccountDTO
{
	public class RegisterDTO
	{
		public string FullName { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string Password { get; set; }
		public string FCM_Token { get; set; } = string.Empty;
		public IFormFile? ProfilePicture { get; set; }
	}
}
