using Autofac.Extras.Moq;
using AutoMapper;
using DevTrack.Infrastructure.Entities;
using DevTrack.Infrastructure.Exceptions;
using DevTrack.Infrastructure.Repositories;
using DevTrack.Infrastructure.Services;
using DevTrack.Infrastructure.UnitOfWorks;
using Moq;
using Shouldly;
using System.Linq.Expressions;
using ProjectBO = DevTrack.Infrastructure.BusinessObjects.Project;
using ProjectEO = DevTrack.Infrastructure.Entities.Project;

namespace DevTrack.Infrastructure.Tests
{
    public class ProjectServiceTests
    {
        private AutoMock _mock;
        private Mock<IApplicationUnitOfWork> _applicationtUnitOfWork;
        private Mock<IProjectRepository> _projectRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IProjectService _projectService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _applicationtUnitOfWork = _mock.Mock<IApplicationUnitOfWork>();
            _projectRepositoryMock = _mock.Mock<IProjectRepository>();
            _mapperMock = _mock.Mock<IMapper>();
            _projectService = _mock.Create<ProjectService>();
        }

        [TearDown]
        public void TearDown()
        {
            _applicationtUnitOfWork.Reset();
            _projectRepositoryMock.Reset();
            _mapperMock.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [Test]
        public void CreateProjectAsync_ProjectDoesNotExists_CreatesProject()
        {
            // Arrange
            ProjectBO project = new ProjectBO
            {
                Title = "Test Project Title"
            };

            ProjectEO projectEntity = new ProjectEO
            {
                Title = "Test Project Title",
                ProjectApplicationUsers = new List<ProjectUser>()
            };

            var userId = new Guid("A5DDE200-5C4B-46E1-9E55-10FD0BF7C721");

            _applicationtUnitOfWork.Setup(x => x.Projects)
                .Returns(_projectRepositoryMock.Object);

            _projectRepositoryMock.Setup(x => x.GetCount(
                It.Is<Expression<Func<ProjectEO, bool>>>(y => y.Compile()(projectEntity))))
                .Returns(0).Verifiable();

            _mapperMock.Setup(x => x.Map<ProjectEO>(project))
                .Returns(projectEntity).Verifiable();

            _projectRepositoryMock.Setup(x => x.AddAsync(It.Is<ProjectEO>(y => y.Title == project.Title)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            _applicationtUnitOfWork.Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            _projectService.CreateProjectAsync(project, userId);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => _applicationtUnitOfWork.VerifyAll(),
                () => _projectRepositoryMock.VerifyAll()
            );
        }

        [Test]
        public void CreateProjectAsync_ProjectExists_ThrowsError()
        {
            // Arrange
            ProjectBO projectBO = new ProjectBO
            {
                Title = "Test Project Title"
            };

            ProjectEO projectEntity = new ProjectEO
            {
                Title = "Test Project Title",
                ProjectApplicationUsers = new List<ProjectUser>()
            };

            var userId = new Guid("A5DDE220-5C4B-46E1-9E55-10FD0BF7C721");

            _applicationtUnitOfWork.Setup(x => x.Projects)
                .Returns(_projectRepositoryMock.Object);

            _projectRepositoryMock.Setup(x => x.GetCount(
                It.Is<Expression<Func<ProjectEO, bool>>>(y => y.Compile()(projectEntity))))
                .Returns(1);

            // Act
            Should.Throw<DuplicateException>(
                () => _projectService.CreateProjectAsync(projectBO, userId)
            );
        }

        [Test]
        public void GetProjectAsync_ValidId_ReturnsProject()
        {
            // Arrange
            var projectId = new Guid("F5DEE230-5C9B-46E1-9E55-16FD0BF7C720");

            ProjectBO projectBO = new ProjectBO
            {
                Id = projectId,
            };

            ProjectEO projectEntity = new ProjectEO
            {
                Id = projectId,
            };

            _applicationtUnitOfWork.Setup(x => x.Projects)
                .Returns(_projectRepositoryMock.Object);

            _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectId)).Returns(Task.FromResult(projectEntity));

            _mapperMock.Setup(x => x.Map<ProjectBO>(projectEntity))
                .Returns(projectBO).Verifiable();

            // Act
            var result = _projectService.GetProjectAsync(projectId);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.Result.ShouldNotBeNull(),
                () => result.Result.Id.ShouldBe(projectId)
            );
        }

        [Test]
        public void EditProject_ProjectBO_UpdatesProject()
        {
            // Arrange
            var projectId = new Guid("F5DE9230-5C9B-46E1-9E55-26FD0BF7C720");

            ProjectBO project = new ProjectBO
            {
                Id = projectId,
                Title = "Test Project Title"
            };

            ProjectEO projectEntity = new ProjectEO
            {
                Id = projectId,
                Title = "Test Project Title",
            };

            _applicationtUnitOfWork.Setup(x => x.Projects)
                .Returns(_projectRepositoryMock.Object);

            _projectRepositoryMock.Setup(x => x.GetCount(
                It.Is<Expression<Func<ProjectEO, bool>>>(y => y.Compile()(projectEntity))))
                .Returns(0).Verifiable();

            _projectRepositoryMock.Setup(x => x.GetByIdAsync(It.Is<Guid>(g => g == projectId)))
                .Returns(Task.FromResult(projectEntity))
                .Verifiable();

            _mapperMock.Setup(x => x.Map<ProjectEO>(project))
                .Returns(projectEntity).Verifiable();

            _applicationtUnitOfWork.Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            _projectService.EditProject(project);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => _applicationtUnitOfWork.VerifyAll(),
                () => _projectRepositoryMock.VerifyAll()
            );
        }
    }
}