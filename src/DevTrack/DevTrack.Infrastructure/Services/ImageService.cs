using DevTrack.Infrastructure.Codes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DevTrack.Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly IPathService _pathService;
        private readonly string _fileSavingPath;
        private readonly IConfiguration _configuration;

        public ImageService(IPathService pathService, IConfiguration configuration)
        {
            _pathService = pathService;
            _configuration = configuration;
            _fileSavingPath = _pathService.GetPath();
        }

        public async Task<string?> SaveImageAsync(string base64String, ImageType imageType)
        {
            if (!string.IsNullOrEmpty(base64String))
            {
                var fileName = $"{imageType}-{Guid.NewGuid()}.jpg";
                string filePath = $"{_fileSavingPath}\\{fileName}";
                await File.WriteAllBytesAsync(filePath, Convert.FromBase64String(base64String));
                return fileName;
            }
            else
            {
                return null;
            }           
        }

        public async Task<string> UploadProfilePictureAsync(IFormFile image)
        {
            var folderName = _configuration.GetValue<string>("AccountImageLocationPath");
            var fileName = $"PP-{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var fileSavingPath = _pathService.GetPath(folderName);
            var filePath = $"{fileSavingPath}\\{fileName}";
            await image.CopyToAsync(new FileStream(filePath, FileMode.Create));

            return fileName;
        }
    }
}
