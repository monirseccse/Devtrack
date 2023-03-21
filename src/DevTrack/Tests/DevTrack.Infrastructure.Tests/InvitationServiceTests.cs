using Autofac.Extras.Moq;
using AutoMapper;
using DevTrack.Infrastructure.Codes;
using DevTrack.Infrastructure.Repositories;
using DevTrack.Infrastructure.Services;
using DevTrack.Infrastructure.UnitOfWorks;
using Moq;
using Shouldly;
using InvitationBO = DevTrack.Infrastructure.BusinessObjects.Invitation;
using InvitationEO = DevTrack.Infrastructure.Entities.Invitation;

namespace DevTrack.Infrastructure.Tests
{
    public class InvitationServiceTests
    {
        private AutoMock _mock;
        private Mock<IApplicationUnitOfWork> _applicationtUnitOfWork;
        private Mock<IInvitationRepository> _invitationRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IInvitationService _invitationService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _applicationtUnitOfWork = _mock.Mock<IApplicationUnitOfWork>();
            _invitationRepositoryMock = _mock.Mock<IInvitationRepository>();
            _mapperMock = _mock.Mock<IMapper>();
            _invitationService = _mock.Create<InvitationService>();
        }

        [TearDown]
        public void TearDown()
        {
            _applicationtUnitOfWork.Reset();
            _invitationRepositoryMock.Reset();
            _mapperMock.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [Test]
        public void CreateInvitationsAsync_InvitationBO_CreatesInvitation()
        {
            // Arrange
            InvitationBO invitation = new InvitationBO
            {
                Email = "test@mail.com"
            };

            InvitationEO invitationEntity = new InvitationEO
            {
                Email = "test@mail.com"
            };

            _mapperMock.Setup(x => x.Map<InvitationEO>(invitation))
                .Returns(invitationEntity).Verifiable();

            _applicationtUnitOfWork.Setup(x => x.Invitations)
                .Returns(_invitationRepositoryMock.Object);

            _invitationRepositoryMock.Setup(x => x.AddAsync(It.Is<InvitationEO>(y => y.Email == invitation.Email)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            _applicationtUnitOfWork.Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            _invitationService.CreateInvitationsAsync(invitation);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => _applicationtUnitOfWork.VerifyAll(),
                () => _invitationRepositoryMock.VerifyAll()
            );
        }

        [Test]
        public void UpdateInvitationStatusAsync_IdAndStatus_UpdatesInvitation()
        {
            // Arrange
            var id = new Guid("25D1E230-5C9B-46E1-9E55-16FE1BF7C720");
            var status = InvitationStatus.Accepted;

            InvitationEO invitationEntity = new InvitationEO
            {
                Id = id,
                Email = "test@mail.com",
                Status = status
            };

            _applicationtUnitOfWork.Setup(x => x.Invitations)
                .Returns(_invitationRepositoryMock.Object);

            _invitationRepositoryMock.Setup(x => x.GetByIdAsync(It.Is<Guid>(g => g == id)))
                .Returns(Task.FromResult(invitationEntity))
                .Verifiable();

            _applicationtUnitOfWork.Setup(x => x.SaveAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            _invitationService.UpdateInvitationStatusAsync(id, status);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => _applicationtUnitOfWork.VerifyAll(),
                () => _invitationRepositoryMock.VerifyAll()
            );
        }

        [Test]
        public void GetInvitationAsync_ValidId_ReturnsInvitation()
        {
            // Arrange
            var invitationId = new Guid("75DEE230-5C9B-46E1-9E55-16FE1BF7C720");

            InvitationBO invitationBO = new InvitationBO
            {
                Id = invitationId,
            };

            InvitationEO invitationEntity = new InvitationEO
            {
                Id = invitationId,
            };

            _applicationtUnitOfWork.Setup(x => x.Invitations)
                .Returns(_invitationRepositoryMock.Object);

            _invitationRepositoryMock.Setup(x => x.GetByIdAsync(invitationId))
                .Returns(Task.FromResult(invitationEntity))
                .Verifiable();

            _mapperMock.Setup(x => x.Map<InvitationBO>(invitationEntity))
                .Returns(invitationBO).Verifiable();

            // Act
            var result = _invitationService.GetInvitationAsync(invitationId);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => result.Result.ShouldNotBeNull(),
                () => result.Result.Id.ShouldBe(invitationId)
            );
        }
    }
}
