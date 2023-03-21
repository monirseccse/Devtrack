using DevTrack.Infrastructure.DbContexts;
using DevTrack.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using ActivityBO = DevTrack.Infrastructure.BusinessObjects.Activity;
using ActivityEO = DevTrack.Infrastructure.Entities.Activity;

namespace DevTrack.Infrastructure.Repositories
{
    public class ReportRepository : Repository<Activity, Guid>, IReportRepository
    {
        public ReportRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }

        public async Task<IList<ActivityEO>> GetActivities(ActivityBO activityBO)
        {
            string[] timesplit = activityBO.DateRange.Split
                (new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            string[] pattern = { "MM/dd/yyyy hh:mm tt", "MM/dd/yyyy hh:mm:ss tt", "M/d/yyyy hh:mm:ss tt" };

            DateTime.TryParseExact(timesplit[0].Trim(), pattern, CultureInfo.CurrentCulture,
                DateTimeStyles.None, out DateTime Starttime);

            DateTime.TryParseExact(timesplit[1].Trim(), pattern, CultureInfo.CurrentCulture,
                DateTimeStyles.None, out DateTime Endtime);

            IList<ActivityEO> activities = await GetAsync(x => x.ApplicationUserId == activityBO.ApplicationUserId
                         && x.ProjectId == activityBO.ProjectId && x.StartTime >= Starttime && x.EndTime <= Endtime,
                         "MouseActivity,ScreenCapture,ActiveWindows,WebcamCapture,RunningPrograms,KeyboardActivity");

            return activities;
        }
    }
}
