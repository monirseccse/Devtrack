using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Services;

namespace DevTrack.Web.Areas.App.Models.Reports
{
    public class ApplicationUserListModel
    {
        public string Name { get; set; }

        private IApplicationUserService _applicationUserService;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        private IHttpContextAccessor? _httpContextAccessor;

        public ApplicationUserListModel()
        {

        }

        public ApplicationUserListModel(IApplicationUserService applicationUserService, IProjectService projectService, 
            IMapper mapper, IHttpContextAccessor? httpContextAccessor)
        {
            _applicationUserService = applicationUserService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _applicationUserService = _scope.Resolve<IApplicationUserService>();
            _mapper = _scope.Resolve<IMapper>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
        }

        public async Task<IList<ApplicationUser>> GetUsersAsync(IList<ProjectUserList> projectUserList)
        {
            IList<ApplicationUser> applicationUser = new List<ApplicationUser>();
            foreach(var projectUser in projectUserList)
            {
                var userId = Guid.Parse(projectUser.ApplicationUserId.ToString());
                var user = await _applicationUserService.GetByUserIdAsync(userId);
                applicationUser.Add(user);
            }
            
            return applicationUser;
        }

        public Guid GetCurrentUserId()
        {
            var currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.
                User.Claims.FirstOrDefault().Value);

            return currentUserId;
        }
    }
}
