using Autofac;
using DevTrack.Web.Areas.App.Models.Projects;
using DevTrack.Web.Areas.App.Models.Reports;
using DevTrack.Web.Models;

namespace DevTrack.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RegisterModel>().AsSelf();
            builder.RegisterType<LoginModel>().AsSelf();
            builder.RegisterType<ProjectCreateModel>().AsSelf();
            builder.RegisterType<ProjectListModel>().AsSelf();
            builder.RegisterType<InvitationCreateModel>().AsSelf();
            builder.RegisterType<ApplicationUserListModel>().AsSelf();
            builder.RegisterType<ReportCreateModel>().AsSelf();
            builder.RegisterType<ProjectArchiveModel>().AsSelf();
            builder.RegisterType<ProjectEditModel>().AsSelf();
            builder.RegisterType<ProjectUserCreateModel>().AsSelf();
            builder.RegisterType<InvitationModel>().AsSelf();
            builder.RegisterType<AccountEditModel>().AsSelf();
            builder.RegisterType<TeamMembersListModel>().AsSelf();
            builder.RegisterType<TrackHourModel>().AsSelf();

            base.Load(builder);
        }
    }
}
