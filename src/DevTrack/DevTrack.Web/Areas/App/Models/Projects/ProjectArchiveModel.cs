using Autofac;
using DevTrack.Infrastructure.Services;

namespace DevTrack.Web.Areas.App.Models.Projects
{
    public class ProjectArchiveModel
    {
        public ProjectArchiveModel(IProjectService projectService,ILifetimeScope scope)
        {
            _projectService = projectService;
            _lifetimeScope = scope;
            _lifetimeScope.Resolve<IProjectService>();
        }

        public IProjectService _projectService { get; }
        public ILifetimeScope _lifetimeScope { get; }

        public async Task ProjectArchive(Guid id, bool isArchived)
        {
           await _projectService.ProjectArchive(id, isArchived);
        }
    }
}
