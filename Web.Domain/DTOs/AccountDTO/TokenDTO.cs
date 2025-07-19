using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Enums;

namespace Web.Domain.DTOs.AccountDTO
{
	public class TokenDTO
	{
		public int StatusCode { get; set; }
		public string Message { get; set; } = string.Empty;
		public string Token { get; set; }
	}
}
