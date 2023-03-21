using Autofac.Extras.Moq;
using AutoMapper;
using DevTrack.Infrastructure.Codes;
using DevTrack.Infrastructure.Repositories;
using DevTrack.Infrastructure.Services;
using DevTrack.Infrastructure.UnitOfWorks;
using Moq;
using Shouldly;
using ApplicationUserBO = DevTrack.Infrastructure.BusinessObjects.ApplicationUser;
using ApplicationUserEO = DevTrack.Infrastructure.Entities.ApplicationUser;

namespace DevTrack.Infrastructure.Tests
{
    public class ApplicationUserServiceTests
    {
        private AutoMock _mock;
        private Mock<IApplicationUnitOfWork> _applicationtUnitOfWork;
        private Mock<IApplicationUserRepository> _appUserRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IApplicationUserService _appUserService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _applicationtUnitOfWork = _mock.Mock<IApplicationUnitOfWork>();
            _appUserRepositoryMock = _mock.Mock<IApplicationUserRepository>();
            _mapperMock = _mock.Mock<IMapper>();
            _appUserService = _mock.Create<ApplicationUserService>();
        }

        [TearDown]
        public void TearDown()
        {
            _applicationtUnitOfWork.Reset();
            _appUserRepositoryMock.Reset();
            _mapperMock.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [Test]
        public void GetByUserIdAsync_ValidId_ReturnsApplicationUser()
        {
            // Arrange
            var userId = new Guid("15DDD230-5C9B-46E1-9E55-16FD0BF7C720");

            ApplicationUserBO appUserBO = new ApplicationUserBO
            {
                Id = userId,
            };

            ApplicationUserEO appUserEntity = new ApplicationUserEO
            {
                Id = userId,
            };

            _applicationtUnitOfWork.Setup(x => x.ApplicationUsers)
                .Returns(_appUserRepositoryMock.Object);

            _appUserRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                .Returns(Task.FromResult(appUserEntity))
                .Verifiable();

            _mapperMock.Setup(x => x.Map<ApplicationUserBO>(appUserEntity))
                .Returns(appUserBO).Verifiable();

            // Act
            var result = _appUserService.GetByUserIdAsync(userId);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.Result.ShouldNotBeNull(),
                () => result.Result.Id.ShouldBe(userId)
            );
        }

        [Test]
        public void EditAccountAsync_ApplicationUserBO_ApplicationUser()
        {
            // Arrange
            var userId = new Guid("B5D1E230-5C9B-46E1-9E55-26FE1BF7C720");

            ApplicationUserBO appUser = new ApplicationUserBO
            {
                Id = userId,
                Name = "Test Name",
                Image = "ImageName.png"
            };

            ApplicationUserEO appUserEntity = new ApplicationUserEO
            {
                Id = userId,
                Name = "Test Name",
                Image = "ImageName.png"
            };

            _applicationtUnitOfWork.Setup(x => x.ApplicationUsers)
                .Returns(_appUserRepositoryMock.Object);

            _appUserRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                .Returns(Task.FromResult(appUserEntity))
                .Verifiable();

            _applicationtUnitOfWork.Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            _appUserService.EditAccountAsync(appUser);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => _applicationtUnitOfWork.VerifyAll(),
                () => _appUserRepositoryMock.VerifyAll()
            );
        }
    }
}
