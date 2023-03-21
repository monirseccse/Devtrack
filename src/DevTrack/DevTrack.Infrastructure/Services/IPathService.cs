namespace DevTrack.Infrastructure.Services
{
    public interface IPathService
    {
        string GetPath(string folderName);
        string GetPath();
    }
}