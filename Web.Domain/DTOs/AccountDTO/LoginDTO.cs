﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.DTOs.AccountDTO
{
	public class LoginDTO
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string FCM_Token { get; set; }
	}
}
