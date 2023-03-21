using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Services;

namespace DevTrack.Web.Areas.App.Models.Reports
{
    public class ReportCreateModel
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string DateRange { get; set; }
        public bool IsOwner { get; set; }

        private IMapper _mapper;
        private ILifetimeScope _scope;
        private IHttpContextAccessor? _httpContextAccessor;
        private IReportService _reportService;
        private IProjectService _projectService;
        public ReportCreateModel()
        {

        }

        public ReportCreateModel(IMapper mapper, IReportService reportService,
            IHttpContextAccessor httpContextAccessor, IProjectService projectService)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _reportService = reportService;
            _projectService = projectService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _mapper = _scope.Resolve<IMapper>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
            _reportService = _scope.Resolve<IReportService>();
        }

        public async Task<IList<Activity>> GetReportsAsync(ReportCreateModel model)
        {
            Activity activity = _mapper.Map<Activity>(this);
            var activities = await _reportService.GetActivities(activity);
            return activities;
        }

        public async Task<bool> IsProjectOwner(Guid projectId)
        {
            var currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.User
                                            .Claims.FirstOrDefault().Value);

            return await _projectService.IsProjectOwner(projectId, currentUserId);
        }
    }
}
