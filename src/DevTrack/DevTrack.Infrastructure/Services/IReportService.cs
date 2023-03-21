using DevTrack.Infrastructure.BusinessObjects;

namespace DevTrack.Infrastructure.Services
{
    public interface IReportService
    {
        Task<IList<Activity>> GetActivities(Activity activity);
    }
}
