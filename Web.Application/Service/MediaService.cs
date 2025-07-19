using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Interfaces;

namespace Web.Application.Service
{
	public class MediaService : IMediaService
	{
		private readonly IConfiguration _configuration;
		public MediaService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task DeleteAsync(string url)
		{
			var imageName = Path.GetFileNameWithoutExtension(url);
			var extention = Path.GetExtension(url);
			var imagePath = $"{_configuration["BaseUrl"]}images/{imageName}{extention}";
			if (File.Exists(imagePath))
			{
				File.Delete(imagePath);
			}

			await Task.CompletedTask;
		}

		public async Task<string> UploadImageAsync(IFormFile Image)
		{
			string extention = Path.GetExtension(Image.FileName);
			string imageName = $"{Guid.NewGuid().ToString()}{extention}";

			string returnedPath = Path.Combine("files", "images");
			string directoryPath = Path.Combine("wwwroot", returnedPath);
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}

			string imagePath = Path.Combine(directoryPath, imageName).Replace('\\', '/');
			using (var stream = new FileStream(imagePath, FileMode.Create))
			{
				Image.CopyTo(stream);
			}
			returnedPath = Path.Combine(returnedPath, imageName).Replace('\\', '/');
			await Task.CompletedTask;
			return _configuration["BaseUrl"] + returnedPath;
		}

	}
}
