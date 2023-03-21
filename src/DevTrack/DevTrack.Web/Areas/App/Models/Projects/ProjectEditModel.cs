using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;

namespace DevTrack.Web.Areas.App.Models.Projects
{
    public class ProjectEditModel
    {
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Title must be provided")]
        public string Title { get; set; }
        public bool AllowScreenshot { get; set; }
        public bool AllowWebcam { get; set; }
        public bool AllowKeyboardHit { get; set; }
        public bool AllowMouseClick { get; set; }
        public bool AllowActiveWindow { get; set; }
        public bool AllowManualTimeEntry { get; set; }
        public bool AllowRunningProgram { get; set; }
        public int TimeInterval { get; set; }
        public bool IsArchived { get; set; }

        private IProjectService _projectService;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        private IHttpContextAccessor _httpContextAccessor;

        public ProjectEditModel()
        {

        }

        public ProjectEditModel(IProjectService projectService, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _projectService = projectService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _projectService = _scope.Resolve<IProjectService>();
            _mapper = _scope.Resolve<IMapper>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
        }

        public async Task<bool> IsProjectOwner(Guid projectId)
        {
            var currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.User
                                            .Claims.FirstOrDefault().Value);

            return await _projectService.IsProjectOwner(projectId, currentUserId);
        }

        public async Task LoadProject(Guid id)
        {
            var project = await _projectService.GetProjectAsync(id);
            _mapper.Map(project, this);
        }

        public async Task UpdateProject(ProjectEditModel model)
        {
            var result = _mapper.Map<Project>(model);
            await _projectService.EditProject(result);
        }
    }
}
