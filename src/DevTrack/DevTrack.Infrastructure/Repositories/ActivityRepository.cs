using DevTrack.Infrastructure.DbContexts;
using DevTrack.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Infrastructure.Repositories
{
    public class ActivityRepository : Repository<Activity, Guid>, IActivityRepository
    {
        public ActivityRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }
    }
}
