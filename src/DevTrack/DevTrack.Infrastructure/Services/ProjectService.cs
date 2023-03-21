using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Codes;
using DevTrack.Infrastructure.Entities;
using DevTrack.Infrastructure.Exceptions;
using DevTrack.Infrastructure.UnitOfWorks;
using ProjectBO = DevTrack.Infrastructure.BusinessObjects.Project;
using ProjectEO = DevTrack.Infrastructure.Entities.Project;

namespace DevTrack.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;

        public ProjectService(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid?> CreateProjectAsync(ProjectBO project, Guid id)
        {
            var count = _applicationUnitOfWork.Projects.GetCount(x => x.Title == project.Title);

            if (count > 0)
            {
                throw new DuplicateException("Project with same name already exists");
            }

            var entity = _mapper.Map<ProjectEO>(project);

            entity.ProjectApplicationUsers.Add(
                new ProjectUser
                {
                    ApplicationUserId = id,
                    Role = ProjectRole.Owner
                });

            await _applicationUnitOfWork.Projects.AddAsync(entity);
            await _applicationUnitOfWork.SaveAsync();

            return entity.Id;
        }

        public async Task<string> RemoveTeamMemberAsync(Guid projectId, Guid userId)
        {
            var projectEOList = await _applicationUnitOfWork.Projects
                .GetAsync(x => x.Id.Equals(projectId), "ProjectApplicationUsers");

            var projectEO = projectEOList.FirstOrDefault();

            if (projectEO != null)
            {
                projectEO.ProjectApplicationUsers.Remove(
                    projectEO.ProjectApplicationUsers.First(u => u.ApplicationUserId == userId));

                await _applicationUnitOfWork.SaveAsync();
            }
            else
                throw new InvalidOperationException("Project was not found");

            return projectEO.Title;
        }

        public async Task<ProjectBO> GetProjectAsync(Guid id)
        {
            var projectEO = await _applicationUnitOfWork.Projects.GetByIdAsync(id);

            var projectBO = _mapper.Map<ProjectBO>(projectEO);

            return projectBO;
        }

        public async Task<IList<ProjectBO>> GetUserProjectsAsync(Guid id)
        {
            var users = await _applicationUnitOfWork.ApplicationUsers.GetAsync(null, "ApplicationUserProjects");
            var projects = new List<ProjectBO>();
            if (users != null)
            {
                var user = users.ToList().Where(a => a.Id == id).FirstOrDefault();
                var userProjects = user.ApplicationUserProjects;
                var projectsEO = new List<ProjectEO>();
                if (user != null && userProjects != null)
                {
                    foreach (var item in userProjects)
                    {
                        projectsEO.Add(await _applicationUnitOfWork.Projects.GetByIdAsync(item.ProjectId));
                    }

                    for (int i = 0; i < projectsEO.Count; i++)
                    {
                        var project = _mapper.Map<ProjectBO>(projectsEO[i]);
                        project.Role = userProjects[i].Role;
                        projects.Add(_mapper.Map<ProjectBO>(project));
                    }
                }
            }
            return projects;
        }

        public (IList<ProjectBO> records, int total, int totalDisplay) GetProjects(int pageIndex,
            int pageSize, string searchText, string orderby, bool status, Guid userid)
        {
            (IList<ProjectEO> records, int total, int totalDisplay) results = _applicationUnitOfWork
                .Projects.GetProjects(pageIndex, pageSize, searchText, orderby, status, userid);

            IList<ProjectBO> projects = new List<ProjectBO>();
            foreach (var item in results.records)
            {
                foreach (var user in item.ProjectApplicationUsers)
                {
                    if (user.ApplicationUserId == userid)
                    {
                        projects.Add(_mapper.Map<ProjectBO>(item));
                    }
                }
            }

            return (projects, results.total, results.totalDisplay);
        }

        public async Task<IList<ProjectBO>> GetAllProjectsAsync()
        {
            IList<ProjectEO> results = await _applicationUnitOfWork
                .Projects.GetAsync(null, "ProjectApplicationUsers,Invitations,Activities");

            IList<ProjectBO> projects = new List<ProjectBO>();
            foreach (var item in results)
            {
                projects.Add(_mapper.Map<ProjectBO>(item));
            }

            return projects;
        }

        public async Task<bool> IsProjectOwner(Guid projectId, Guid userId)
        {
            var projectEOList = await _applicationUnitOfWork.Projects
                .GetAsync(x => x.Id.Equals(projectId), "ProjectApplicationUsers");

            var projectEO = projectEOList.FirstOrDefault();

            var owner = projectEO?.ProjectApplicationUsers.FirstOrDefault(
                                    u => u.ApplicationUserId == userId && u.Role == ProjectRole.Owner);

            return owner != null;
        }

        public async Task EditProject(ProjectBO project)
        {
            var count = _applicationUnitOfWork.Projects.GetCount(x => x.Title == project.Title);

            if (count > 0)
            {
                throw new DuplicateException("Project with same name already exists");
            }

            var projectEntity = await _applicationUnitOfWork.Projects.GetByIdAsync(project.Id);
            if (projectEntity != null)
            {
                var createdDate = projectEntity.CreatedDate;
                _mapper.Map(project, projectEntity);
                projectEntity.CreatedDate = createdDate;

                await _applicationUnitOfWork.SaveAsync();
            }
            else
                throw new InvalidOperationException("Project was not found");
        }

        public async Task ProjectArchive(Guid id, bool isArchived)
        {
            var projectEntity = await _applicationUnitOfWork.Projects.GetByIdAsync(id);
            if (projectEntity != null)
            {
                projectEntity.IsArchived = isArchived;

                await _applicationUnitOfWork.SaveAsync();
            }
            else
                throw new InvalidOperationException("Project was not found");
        }

        public async Task CreateProjectUserAsync(ProjectBO project, Guid userId)
        {
            var projectEOList = await _applicationUnitOfWork.Projects
                .GetAsync(x => x.Id.Equals(project.Id), "ProjectApplicationUsers");

            var projectEO = projectEOList.FirstOrDefault();
            if (projectEO != null)
            {
                var isJoined = false;
                foreach (var projectUser in projectEO.ProjectApplicationUsers)
                {
                    if (projectUser.ApplicationUserId == userId)
                    {
                        isJoined = true;
                        break;
                    }
                }
                if (!isJoined)
                {
                    projectEO.ProjectApplicationUsers.Add(
                        new ProjectUser
                        {
                            ApplicationUserId = userId,
                            Role = ProjectRole.Worker
                        });
                    await _applicationUnitOfWork.SaveAsync();
                }
            }
            else
                throw new InvalidOperationException("Project was not found");
        }

        public async Task<IList<ProjectUserList>> GetTeamMembersAsync(Guid projectId)
        {
            var projectEOList = await _applicationUnitOfWork.Projects
                .GetAsync(x => x.Id.Equals(projectId), "ProjectApplicationUsers");

            var projectEO = projectEOList.FirstOrDefault();
            if (projectEO != null)
            {
                var teamMembers = new List<ProjectUserList>();
                foreach (var projectUser in projectEO.ProjectApplicationUsers)
                {
                    teamMembers.Add(_mapper.Map<ProjectUserList>(projectUser));
                }

                return teamMembers;
            }
            else
                throw new InvalidOperationException("Project was not found");
        }
    }
}
