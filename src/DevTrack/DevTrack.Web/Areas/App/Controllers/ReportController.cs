using Autofac;
using DevTrack.Web.Areas.App.Models.Projects;
using DevTrack.Web.Areas.App.Models.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DevTrack.Web.Areas.App.Controllers
{
    [Area("App")]
    [Authorize]
    public class ReportController : Controller
    {
        private readonly ILogger<ReportController> _logger;
        private readonly ILifetimeScope _scope;

        public ReportController(ILogger<ReportController> logger, ILifetimeScope scope)
        {
            _logger = logger;
            _scope = scope;
        }

        public async Task<IActionResult> Index(Guid projectId, string projectName)
        {
            var model = _scope.Resolve<ReportCreateModel>();
            model.ProjectId = projectId;
            model.ProjectName = projectName;
            
            return View(model);
        }
        public async Task<object> GetProjectNameAsync()
        {
            var model = _scope.Resolve<ProjectListModel>();
            var projects = await model.GetProjectsAsync(true);
            var json = JsonSerializer.Serialize(projects);
            return json;
        }
        
        [HttpGet]
        public async Task<object> GetApplicationUserNameAsync(Guid projectId)
        {
            var model = _scope.Resolve<ProjectListModel>();
            var projectUserList = await model.GetProjectUsersAsync(projectId);
            var userModel = _scope.Resolve<ApplicationUserListModel>();
            var users = await userModel.GetUsersAsync(projectUserList);
            var json = JsonSerializer.Serialize(users);
            return json;
        }

        [HttpGet]
        public async Task<object> GetSearchedDataAsync(ReportCreateModel model)
        {
            model.ResolveDependency(_scope);
            var reportActivity = await model.GetReportsAsync(model);
            var json = JsonSerializer.Serialize(reportActivity);
            return json;
        }

        public async Task<IActionResult> TrackedHour(Guid projectId, string projectName)
        {
            var model = _scope.Resolve<ReportCreateModel>();
            model.ProjectId = projectId;
            model.ProjectName = projectName;
            model.IsOwner = await model.IsProjectOwner(projectId);

            return View(model);
        }

        public async Task<object> GetTrackedHoursAsync(Guid id)
        {
            var TrackedHourModel = _scope.Resolve<TrackHourModel>();
            var TrackedData = TrackedHourModel.GetTrackedHoursAsync(id);
            return JsonSerializer.Serialize(TrackedData.Result);
        }
    }
}
