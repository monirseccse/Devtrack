using Autofac;
using DevTrack.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevTrack.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<ActivitiesController> _logger;

        public ActivitiesController(ILogger<ActivitiesController> logger, ILifetimeScope scope)
        {
            _logger = logger;
            _scope = scope;
        }

        [HttpPost]
        public async Task<ProjectResponse> Post(ActivityDataModel model)
        {
            var projectResponse = _scope.Resolve<ProjectResponse>();
            try
            {
                if (ModelState.IsValid)
                {
                    model.ResolveDependency(_scope);
                    projectResponse.isSuccess = await model.SaveActivityDataAsync();
                    projectResponse.statusCode = 200;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to post activities", ex.Message);
                projectResponse.isSuccess = false;
                projectResponse.statusCode = 502;
                projectResponse.errors = new string[] { "Operation Failed" };
            }

            return projectResponse;
        }
    }
}
