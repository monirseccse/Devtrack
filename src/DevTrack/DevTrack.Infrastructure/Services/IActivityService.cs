using DevTrack.Infrastructure.BusinessObjects;

namespace DevTrack.Infrastructure.Services
{
    public interface IActivityService
    {
        Task<bool> SaveActivityAsync(Activity activityData);
    }
}