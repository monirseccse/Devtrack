using Autofac;
using DevTrack.Infrastructure.DbContexts;
using DevTrack.Infrastructure.Repositories;
using DevTrack.Infrastructure.Services;
using DevTrack.Infrastructure.Services.Email;
using DevTrack.Infrastructure.Services.Encryption;
using DevTrack.Infrastructure.UnitOfWorks;

namespace DevTrack.Infrastructure
{
    public class InfrastructureModule: Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public InfrastructureModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationDbContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationDbContext>().As<IApplicationDbContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<TimeService>().As<ITimeService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TokenService>().As<ITokenService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUnitOfWork>().As<IApplicationUnitOfWork>()
                .InstancePerLifetimeScope();            

            builder.RegisterType<ProjectService>().As<IProjectService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ReportService>().As<IReportService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProjectRepository>().As<IProjectRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ReportRepository>().As<IReportRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUserService>().As<IApplicationUserService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ApplicationUserRepository>().As<IApplicationUserRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ActivityService>().As<IActivityService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ActivityRepository>().As<IActivityRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<InvitationService>().As<IInvitationService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<InvitationRepository>().As<IInvitationRepository>()
                .InstancePerLifetimeScope();           

            builder.RegisterType<HtmlEmailService>().As<IEmailService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SymmetricEncryptionService>().As<ISymmetricEncryptionService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ImageService>().As<IImageService>()
               .InstancePerLifetimeScope();

            builder.RegisterType<PathService>().As<IPathService>()
               .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
