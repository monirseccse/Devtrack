using Autofac;
using DevTrack.Api.Model;

namespace DevTrack.Api
{
    public class ApiModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProjectModel>().AsSelf();
            builder.RegisterType<ActivityDataModel>().AsSelf();
            builder.RegisterType<ProjectResponse>().AsSelf();

            base.Load(builder);
        }
    }
}
