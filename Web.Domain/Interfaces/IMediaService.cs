using Microsoft.AspNetCore.Http;

namespace Web.Domain.Interfaces
{
    public interface IMediaService
    {
        Task<string> UploadImageAsync(IFormFile? media);
        //public string UploadImage(IFormFile File, string FolderName);
        Task DeleteAsync(string url);
    }
}
