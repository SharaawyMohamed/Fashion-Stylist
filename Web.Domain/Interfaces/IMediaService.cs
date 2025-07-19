using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Domain.Interfaces
{
	public interface IMediaService
	{
		Task<string> UploadImageAsync(IFormFile? media);

		Task DeleteAsync(string url);
	}
}
