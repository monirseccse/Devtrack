using DevTrack.Infrastructure.DbContexts;
using DevTrack.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Infrastructure.UnitOfWorks
{
    public class ApplicationUnitOfWork : UnitOfWork, IApplicationUnitOfWork
    {
        public IProjectRepository Projects { get; private set; }
        public IInvitationRepository Invitations { get; private set; }
        public IApplicationUserRepository ApplicationUsers { get; private set; }
        public IReportRepository Reports { get; private set; }
        public IActivityRepository Activities { get; private set; }
        public ApplicationUnitOfWork(IApplicationDbContext dbContext,
            IProjectRepository projects,
            IApplicationUserRepository applicationUsers,
            IInvitationRepository invitations,
            IReportRepository reportRepository,
            IActivityRepository activities) : base((DbContext)dbContext)
        {
            Projects = projects;
            ApplicationUsers = applicationUsers;
            Invitations = invitations;
            Reports = reportRepository;
            Activities = activities;
        }      
    }
}
