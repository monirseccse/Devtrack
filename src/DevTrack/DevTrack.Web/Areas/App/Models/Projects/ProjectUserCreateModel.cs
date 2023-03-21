using Autofac;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Services;
using DevTrack.Infrastructure.Services.Encryption;

namespace DevTrack.Web.Areas.App.Models.Projects
{
    public class ProjectUserCreateModel
    {
        public string ApplicationUserId { get; set; }
        public string ProjectId { get; set; }

        private ILifetimeScope _scope;
        private IProjectService _projectService;
        private ISymmetricEncryptionService _symmetricEncryptionService;

        public ProjectUserCreateModel()
        {

        }

        public ProjectUserCreateModel(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _projectService = _scope.Resolve<IProjectService>();
            _symmetricEncryptionService = _scope.Resolve<ISymmetricEncryptionService>();
        }

        public async Task CreateProjectUserAsync()
        {
            var project = new Project();
            var decryptedProjectId = await _symmetricEncryptionService.Decrypt(ProjectId);
            project.Id = Guid.Parse(decryptedProjectId);

            await _projectService.CreateProjectUserAsync(project, Guid.Parse(ApplicationUserId));
        }
    }
}
