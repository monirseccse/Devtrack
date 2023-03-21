using Autofac;
using DevTrack.Worker.Models;
using DevTrack.Worker.WorkerServices;
using Microsoft.AspNetCore.Routing;

namespace DevTrack.Worker
{
    public  class WorkerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailSender>().AsSelf();
            builder.RegisterType<InvitationModel>().AsSelf();
            builder.RegisterType<ProjectModel>().AsSelf();

            base.Load(builder);
        }
    }
}
