using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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

        public async Task<string> UploadImageAsync(IFormFile Image, string FolderName = "images")
        {
            string extention = Path.GetExtension(Image.FileName);
            string imageName = $"{Guid.NewGuid().ToString()}{extention}";

            string returnedPath = Path.Combine("files", FolderName);
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

        //public string UploadImage(IFormFile File, string FolderName)
        //{

        //    string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);

        //    string FileName = $"{Guid.NewGuid()}{File.FileName.Replace(" ", "_")}";

        //    string FilePath = Path.Combine(FolderPath, FileName);
        //    Directory.CreateDirectory(FolderPath);
        //    using var FS = new FileStream(FilePath, FileMode.Create);
        //    File.CopyTo(FS);

        //    return FileName;
        //}

    }
}
