using DevTrack.Infrastructure.Entities;

namespace DevTrack.Infrastructure.Repositories
{
    public interface IProjectRepository : IRepository<Project, Guid>
    {
        (IList<Project> records, int total, int totalDisplay) GetProjects(int pageIndex,
            int pageSize, string searchText, string orderby, bool status, Guid userid);
    }
}
