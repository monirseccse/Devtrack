using Autofac;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Services;

namespace DevTrack.Worker.Models
{
    public class ProjectModel
    {
        protected ILifetimeScope? _scope;
        private IProjectService? _projectService;

        public void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _projectService = _scope.Resolve<IProjectService>();
        }

        internal async Task<Project> GetProjectByIDAsync(Guid id)
        {
            return await _projectService.GetProjectAsync(id);
        }
    }
}
