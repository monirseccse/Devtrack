using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Services;
using Org.BouncyCastle.Ocsp;

namespace DevTrack.Web.Areas.App.Models.Projects
{
    public class TeamMembersListModel
    {
        private IProjectService _projectService;
        private IApplicationUserService _applicationUserService;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        private IHttpContextAccessor? _httpContextAccessor;
        private readonly string _path;

        public TeamMembersListModel()
        {

        }

        public TeamMembersListModel(IProjectService projectService,
            IApplicationUserService applicationUserService,
            IMapper mapper,
            IHttpContextAccessor? httpContextAccessor,
            IConfiguration configuration)
        {
            _projectService = projectService;
            _applicationUserService = applicationUserService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _path = configuration.GetValue<string>("AccountImageLocationPath");
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _projectService = _scope.Resolve<IProjectService>();
            _applicationUserService = _scope.Resolve<IApplicationUserService>();
            _mapper = _scope.Resolve<IMapper>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
        }

        public async Task<object?> GetTeamMembersAsync(Guid projectId)
        {
            var team = await _projectService.GetTeamMembersAsync(projectId);
            var records = new List<TeamMember>();

            foreach (var projectUser in team)
            {
                var userId = Guid.Parse(projectUser.ApplicationUserId.ToString());
                var user = _applicationUserService.GetByUserId(userId);
                var member = new TeamMember();
                member.Name = user.Name;
                member.Image = _path + user.Image;
                member.Role = projectUser.Role.ToString();
                member.MemberId = userId;

                if (member.Role == "Owner")
                    records.Insert(0, member);
                else
                    records.Add(member);
            }

            return new
            {
                data = (from record in records
                        select new string[]
                        {
                                record.Name,
                                record.Image,
                                record.Role,
                                record.MemberId.ToString()
                        }
                    ).ToArray()
            };
        }

        public async Task<string> RemoveMember(Guid projectId, Guid userId)
        {
            return await _projectService.RemoveTeamMemberAsync(projectId, userId);
        }
    }
}
