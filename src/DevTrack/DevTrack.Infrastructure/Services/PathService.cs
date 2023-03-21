using Microsoft.Extensions.Configuration;

namespace DevTrack.Infrastructure.Services
{
    public class PathService : IPathService
    {
        private readonly IConfiguration _configuration;

        public PathService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetPath(string folderName)
        {
            var rootPath = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent;
            var fullPath = Path.Combine(rootPath.FullName, @"DevTrack.Web\wwwroot\" + folderName);
            
            if(!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
            
            return fullPath;
        }

        public string GetPath()
        {
            var rootPath = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent;
            var folderName = _configuration.GetValue<string>("ImageLocationPath");
            var fullPath = Path.Combine(rootPath.FullName, @"DevTrack.Web\wwwroot\" + folderName);

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            return fullPath;
        }
    }
}
