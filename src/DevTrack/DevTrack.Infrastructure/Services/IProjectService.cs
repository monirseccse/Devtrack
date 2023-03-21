using DevTrack.Infrastructure.BusinessObjects;

namespace DevTrack.Infrastructure.Services
{
    public interface IProjectService
    {
        Task<Guid?> CreateProjectAsync(Project project, Guid userId);
        Task<Project> GetProjectAsync(Guid id);
        Task<IList<Project>> GetAllProjectsAsync();
        Task<IList<Project>> GetUserProjectsAsync(Guid id);
        (IList<Project> records, int total, int totalDisplay) GetProjects(int pageIndex,
         int pageSize, string searchText, string orderby, bool status, Guid userid);

        Task<bool> IsProjectOwner(Guid projectId, Guid userId);
        Task EditProject(Project project);
        Task ProjectArchive(Guid id, bool isArchived);
        Task CreateProjectUserAsync(Project project, Guid userId);
        Task<IList<ProjectUserList>> GetTeamMembersAsync(Guid projectId);
        Task<string> RemoveTeamMemberAsync(Guid projectId, Guid userId);
    }
}
