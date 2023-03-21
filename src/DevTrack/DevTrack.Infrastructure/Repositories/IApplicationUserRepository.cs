using DevTrack.Infrastructure.Entities;

namespace DevTrack.Infrastructure.Repositories
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser, Guid>
    {
    }
}
