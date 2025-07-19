﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.DTOs.EmailDTO
{
	public class EmailDto
	{
		public string Email { get; set; }
		public string DisplayName { get; set; }
		public string Password { get; set; }
		public string Host { get; set; }
		public int Port { get; set; }
		public bool EnableSSL { get; set; }
		public bool UseDefaultCredentials { get; set; }
	}
}
