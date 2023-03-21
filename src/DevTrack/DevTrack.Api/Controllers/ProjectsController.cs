using Autofac;
using DevTrack.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevTrack.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<ProjectsController> _logger;

        public ProjectsController(ILogger<ProjectsController> logger, ILifetimeScope scope)
        {
            _logger = logger;
            _scope = scope;
        }

        [HttpGet]
        public async Task<ProjectResponse> Get()
        {
            var projectResponse = new ProjectResponse();
            if (User.Claims.Count() != 0)
            {
                var id = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId").Value);

                try
                {
                    var model = _scope.Resolve<ProjectModel>();
                    var projects = await model.GetProjects(id);
                    projectResponse.data = (List<ProjectData>)projects;
                    projectResponse.statusCode = 200;
                    projectResponse.isSuccess = true;
                    projectResponse.errors = null;

                }

                catch (Exception ex)
                {
                    _logger.LogError(ex, "Couldn't get projects");
                    projectResponse.statusCode = 500;
                    projectResponse.isSuccess = false;
                    projectResponse.errors = new string[] { "Internal server error!" };                    
                }
            }

            else
            {
                projectResponse.data = null;
                projectResponse.statusCode = 401;
                projectResponse.isSuccess = false;
                projectResponse.errors = new string[] { "Invalid token found" };
            }

            return projectResponse;
        }
    }
}
