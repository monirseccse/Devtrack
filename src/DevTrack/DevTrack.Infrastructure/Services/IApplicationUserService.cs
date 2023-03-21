using DevTrack.Infrastructure.BusinessObjects;

namespace DevTrack.Infrastructure.Services
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser> GetByUserIdAsync(Guid Id);
        ApplicationUser GetByUserId(Guid Id);
        Task EditAccountAsync(ApplicationUser user);
    }
}
