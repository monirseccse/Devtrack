using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Codes;
using DevTrack.Infrastructure.Services;
using DevTrack.Web.Models;

namespace DevTrack.Web.Areas.App.Models.Projects
{
    public class ProjectListModel
    {
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

        private IProjectService _projectService;
        private IMapper _mapper;
        private ILifetimeScope _scope;
        private IHttpContextAccessor? _httpContextAccessor;
        private IApplicationUserService _applicationUserService;

        public ProjectListModel()
        {

        }

        public ProjectListModel(IProjectService projectService, 
            IMapper mapper, IHttpContextAccessor? httpContextAccessor,
            IApplicationUserService applicationUserService)
        {
            _projectService = projectService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _applicationUserService = applicationUserService;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _projectService = _scope.Resolve<IProjectService>();
            _mapper = _scope.Resolve<IMapper>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
        }

        public object? GetPagedProjects(DataTablesAjaxRequestModel model, bool status)
        {
            var currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.
                User.Claims.FirstOrDefault().Value);

            var data = _projectService.GetProjects(
                model.PageIndex,
                model.PageSize,
                model.SearchText,
                model.GetSortText(new string[] { "Title", "CreatedDate" }), status, currentUserId);

            return new
            {
                recordsTotal = data.records.Count,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.Title,
                                record.ProjectApplicationUsers.Count().ToString(),
                                record.CreatedDate.ToString(),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }

        public async Task<IList<Project>> GetProjectsAsync(bool excludeArchives)
        {
            var currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.
                User.Claims.FirstOrDefault().Value);

            var projects = await _projectService.GetAllProjectsAsync();
            var userProjects = new List<Project>();

            foreach (var project in projects)
            {
                if (excludeArchives)
                {
                    if (!project.IsArchived)
                    {
                        var projectUser = project.ProjectApplicationUsers;
                        foreach (var user in projectUser)
                        {
                            if (user.ApplicationUserId == currentUserId && user.Role == ProjectRole.Owner)
                            {
                                userProjects.Add(project);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    var projectUser = project.ProjectApplicationUsers;
                    foreach (var user in projectUser)
                    {
                        if (user.ApplicationUserId == currentUserId && user.Role == ProjectRole.Owner)
                        {
                            userProjects.Add(project);
                            break;
                        }
                    }
                }
            }

            return userProjects;
        }

        public async Task<object> GetOwnerProjectIdListAsync()
        {
            var ownerProjects = await GetProjectsAsync(false);
            return new
            {
                data = (from record in ownerProjects
                        select record.Id.ToString()).ToArray()
            };
        }

        public async Task<IList<ProjectUserList>> GetProjectUsersAsync(Guid projectId)
        {
            var projectUserList = new List<ProjectUserList>();
            var projects = await GetProjectsAsync(true);

            foreach (var project in projects)
            {
                if (projectId == project.Id)
                {
                    projectUserList = project.ProjectApplicationUsers;
                }
            }

            return projectUserList;
        }

        public async Task<IList<Project>> GetCurrentUserProjectsAsync(Guid currentUserId)
        {
            var projects = await _projectService.GetAllProjectsAsync();
            var userProjects = new List<Project>();

            foreach (var project in projects)
            {
                if (!project.IsArchived)
                {
                    foreach (var user in project.ProjectApplicationUsers)
                    {
                        if (user.ApplicationUserId == currentUserId)
                        {
                            userProjects.Add(project);
                            break;
                        }
                    }
                }
            }

            return userProjects;
        }

        public async Task<DateTime> GetProjectCreatedDate(Guid projectId)
        {
            var projects = await _projectService.GetAllProjectsAsync();
            var createdDate =  DateTime.UtcNow;
            foreach(var project in projects)
            {
                if(projectId == project.Id)
                {
                    createdDate = project.CreatedDate;
                    break;
                }
            }

            return createdDate;
        }

        public async Task<List<TeamMember>> GetTeamMembersAsync(Guid projectId)
        {
            var currentUserId = Guid.Parse(_httpContextAccessor.HttpContext.
                User.Claims.FirstOrDefault().Value);

            var team = await _projectService.GetTeamMembersAsync(projectId);
            var records = new List<TeamMember>();

            foreach (var projectUser in team)
            {
                var userId = Guid.Parse(projectUser.ApplicationUserId.ToString());
                var user = await _applicationUserService.GetByUserIdAsync(userId);
                var member = _mapper.Map<TeamMember>(user);
                member.Role = projectUser.Role.ToString();
                member.MemberId = userId;
                records.Add(member);
            }

            var teamMemberDetails = new List<TeamMember>();
            foreach(var record in records)
            {
                teamMemberDetails.Add(record);
                if(record.MemberId == currentUserId && record.Role == "Worker")
                {
                    teamMemberDetails.Clear();
                    teamMemberDetails.Add(record);
                    break;
                }
            }

            return teamMemberDetails;
        }
    }
}
