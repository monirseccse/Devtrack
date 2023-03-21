using Autofac.Extras.Moq;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Codes;
using DevTrack.Infrastructure.Repositories;
using DevTrack.Infrastructure.Services;
using DevTrack.Infrastructure.UnitOfWorks;
using Moq;
using Shouldly;
using ActivityBO = DevTrack.Infrastructure.BusinessObjects.Activity;
using ActivityEO = DevTrack.Infrastructure.Entities.Activity;

namespace DevTrack.Infrastructure.Tests
{
    public class ActivityServiceTests
    {
        private AutoMock _mock;
        private Mock<IApplicationUnitOfWork> _applicationtUnitOfWork;
        private Mock<IActivityRepository> _activityRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IImageService> _imageServiceMock;
        private IActivityService _activityService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _applicationtUnitOfWork = _mock.Mock<IApplicationUnitOfWork>();
            _activityRepositoryMock = _mock.Mock<IActivityRepository>();
            _mapperMock = _mock.Mock<IMapper>();
            _imageServiceMock = _mock.Mock<IImageService>();
            _activityService = _mock.Create<ActivityService>();
        }

        [TearDown]
        public void TearDown()
        {
            _applicationtUnitOfWork.Reset();
            _activityRepositoryMock.Reset();
            _mapperMock.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [Test]
        public void SaveActivityAsync_ActivityBO_CreatesActivity()
        {
            // Arrange
            ActivityBO activity = new ActivityBO
            {
                Description = "Test Description",
                ScreenCapture = new ScreenCapture
                {
                    Image = "screenCaptureName.png"
                },
                WebcamCapture = new WebcamCapture
                {
                    Image = "webcamCaptureName.png"
                }
            };

            ActivityEO activityEntity = new ActivityEO
            {
                Description = "Test Description",
                ScreenCapture = new Entities.ScreenCapture
                {
                    Image = "screenCaptureName.png"
                },
                WebcamCapture = new Entities.WebcamCapture
                {
                    Image = "webcamCaptureName.png"
                }
            };

            _mapperMock.Setup(x => x.Map<ActivityEO>(activity))
                .Returns(activityEntity).Verifiable();

            _imageServiceMock.Setup(s => s.SaveImageAsync(
                It.Is<string>(st => st == activityEntity.ScreenCapture.Image), ImageType.ScreenCapture))
                .Returns(Task.FromResult(activityEntity.ScreenCapture.Image))
                .Verifiable();

            _imageServiceMock.Setup(s => s.SaveImageAsync(
                It.Is<string>(st => st == activityEntity.WebcamCapture.Image), ImageType.WebcamCapture))
                .Returns(Task.FromResult(activityEntity.WebcamCapture.Image))
                .Verifiable();

            _applicationtUnitOfWork.Setup(x => x.Activities)
                .Returns(_activityRepositoryMock.Object);

            _activityRepositoryMock.Setup(x => x.AddAsync(It.Is<ActivityEO>(y => y.Description == activity.Description)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            _applicationtUnitOfWork.Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            _activityService.SaveActivityAsync(activity);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => _applicationtUnitOfWork.VerifyAll(),
                () => _activityRepositoryMock.VerifyAll()
            );
        }
    }
}
