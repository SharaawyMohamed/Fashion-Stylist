﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.DTOs.AccountDTO
{
	public class ResetPasswordDto
	{
		public string Email { get; set; }
		public string Token { get; set; }
		public string NewPassword { get; set; }
		public string ConfirmNewPassword { get; set; }
	}
}
