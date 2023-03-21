using DevTrack.Infrastructure.DbContexts;
using DevTrack.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Infrastructure.Repositories
{
    public class InvitationRepository : Repository<Invitation, Guid>, IInvitationRepository
    {
        public InvitationRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }
    }
}
