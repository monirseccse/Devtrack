using DevTrack.Infrastructure.Entities;
using ActivityBO = DevTrack.Infrastructure.BusinessObjects.Activity;
using ActivityEO = DevTrack.Infrastructure.Entities.Activity;

namespace DevTrack.Infrastructure.Repositories
{
    public interface IReportRepository : IRepository<Activity, Guid>
    {
        Task<IList<ActivityEO>> GetActivities(ActivityBO activity);
    }
}
