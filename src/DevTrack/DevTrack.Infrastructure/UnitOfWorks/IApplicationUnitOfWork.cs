using DevTrack.Infrastructure.Repositories;

namespace DevTrack.Infrastructure.UnitOfWorks
{
    public interface IApplicationUnitOfWork : IUnitOfWork
    {
        IProjectRepository Projects { get; }
        IApplicationUserRepository ApplicationUsers { get; }
        IInvitationRepository Invitations { get; }
        IReportRepository Reports { get; }
        IActivityRepository Activities { get; }
    }
}
