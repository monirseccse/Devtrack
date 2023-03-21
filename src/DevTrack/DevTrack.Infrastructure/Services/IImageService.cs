using DevTrack.Infrastructure.Codes;
using Microsoft.AspNetCore.Http;

namespace DevTrack.Infrastructure.Services
{
    public interface IImageService
    {
        Task<string?> SaveImageAsync(string base64String, ImageType imageType);
        Task<string?> UploadProfilePictureAsync(IFormFile image);
    }
}
