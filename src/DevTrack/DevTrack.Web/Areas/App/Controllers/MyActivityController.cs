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
    public class MyActivityController : Controller
    {
        private readonly ILogger<MyActivityController> _logger;
        private readonly ILifetimeScope _scope;

        public MyActivityController(ILogger<MyActivityController> logger, ILifetimeScope scope)
        {
            _logger = logger;
            _scope = scope;
        }
        public IActionResult Index(Guid projectId, string projectName)
        {
            var model = _scope.Resolve<ReportCreateModel>();
            model.ProjectId = projectId;
            model.ProjectName = projectName;

            return View(model);
        }

        public async Task<object> GetProjectNameAsync()
        {
            var userModel = _scope.Resolve<ApplicationUserListModel>();
            var currentuserId = userModel.GetCurrentUserId();
            var model = _scope.Resolve<ProjectListModel>();
            var projects = await model.GetCurrentUserProjectsAsync(currentuserId);
            var json = JsonSerializer.Serialize(projects);
            return json;
        }

        public async Task<object> GetApplicationUserNameAsync(Guid projectId)
        {
            var model = _scope.Resolve<ProjectListModel>();
            var projectUserList = await model.GetProjectUsersAsync(projectId);
            var userModel = _scope.Resolve<ApplicationUserListModel>();
            var users = await userModel.GetUsersAsync(projectUserList);
            var json = JsonSerializer.Serialize(users);
            return json;
        }

        public object GetCurrentUserId()
        {
            var userModel = _scope.Resolve<ApplicationUserListModel>();
            var currentuserId = userModel.GetCurrentUserId();
            var json = JsonSerializer.Serialize(currentuserId);
            return json;
        }

        public async Task<object> GetSearchedDataAsync(ReportCreateModel model)
        {
            model.ResolveDependency(_scope);
            var reportActivity = await model.GetReportsAsync(model);
            var json = JsonSerializer.Serialize(reportActivity);
            return json;
        }
    }
}
