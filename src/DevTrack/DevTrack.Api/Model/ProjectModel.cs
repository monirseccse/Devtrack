using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.Services;

namespace DevTrack.Api.Model
{
    public class ProjectModel
    {
        private IProjectService? _projectService;
        private IMapper _mapper;

        public Guid Id { get; set; }
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
        public DateTime CreatedDate { get; set; }

        public ProjectModel()
        {

        }

        public ProjectModel(IProjectService projectService, IMapper mapper)
        {
            _projectService = projectService;
            _mapper = mapper;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _projectService = scope.Resolve<IProjectService>();
            _mapper = scope.Resolve<IMapper>();
        }

        internal async Task<IList<ProjectData>> GetProjects(Guid id)
        {
            var projects = await _projectService.GetUserProjectsAsync(id);
            var data = new List<ProjectData>();
            foreach (var project in projects)
            {
                data.Add(new ProjectData()
                {
                    ProjectId = project.Id,
                    Title = project.Title,
                    AllowWebcam = project.AllowWebcam,
                    AllowScreenshot = project.AllowScreenshot,
                    AllowKeyboardHit = project.AllowKeyboardHit,
                    AllowMouseClick = project.AllowMouseClick,
                    AllowActiveWindow = project.AllowActiveWindow,
                    AllowRunningProgram = project.AllowRunningProgram,
                    AllowManualTimeEntry = project.AllowManualTimeEntry,
                    TimeInterval = project.TimeInterval,
                    CreatedDate = project.CreatedDate,
                    Role = (int)project.Role
                });
            }
            return data;
        }
    }
}