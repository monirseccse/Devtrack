using DevTrack.Infrastructure.DbContexts;
using DevTrack.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project, Guid>, IProjectRepository
    {
        public ProjectRepository(IApplicationDbContext context) : base((DbContext)context)
        {
        }

        public (IList<Project> records, int total, int totalDisplay) GetProjects(int pageIndex,
            int pageSize, string searchText, string orderby, bool status, Guid userid)
        {
            (IList<Project> records, int total, int totalDisplay) results = GetDynamic(
                 x => x.Title.Contains(searchText) && x.IsArchived == status
                 && x.ProjectApplicationUsers.Any(x => x.ApplicationUserId == userid), orderby,
                "ProjectApplicationUsers,Invitations,Activities", pageIndex, pageSize, true);

            return results;
        }

    }
}
