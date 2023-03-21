using DevTrack.Infrastructure.DbContexts;
using DevTrack.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Infrastructure.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser, Guid>, IApplicationUserRepository
    {
        public ApplicationUserRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }
    }
}
