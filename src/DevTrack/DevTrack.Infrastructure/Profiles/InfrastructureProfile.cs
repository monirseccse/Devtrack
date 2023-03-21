using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Entities;
using ProjectBO = DevTrack.Infrastructure.BusinessObjects.Project;
using ProjectEO = DevTrack.Infrastructure.Entities.Project;
using InvitationBO = DevTrack.Infrastructure.BusinessObjects.Invitation;
using InvitationEO = DevTrack.Infrastructure.Entities.Invitation;
using ActiveWindowBO = DevTrack.Infrastructure.BusinessObjects.ActiveWindow;
using ActiveWindowEO = DevTrack.Infrastructure.Entities.ActiveWindow;
using ActivityBO = DevTrack.Infrastructure.BusinessObjects.Activity;
using ActivityEO = DevTrack.Infrastructure.Entities.Activity;
using ApplicationUserBO = DevTrack.Infrastructure.BusinessObjects.ApplicationUser;
using ApplicationUserEO = DevTrack.Infrastructure.Entities.ApplicationUser;
using KeyboardActivityBO = DevTrack.Infrastructure.BusinessObjects.KeyboardActivity;
using KeyboardActivityEO = DevTrack.Infrastructure.Entities.KeyboardActivity;
using MouseActivityBO = DevTrack.Infrastructure.BusinessObjects.MouseActivity;
using MouseActivityEO = DevTrack.Infrastructure.Entities.MouseActivity;
using RunningProgramBO = DevTrack.Infrastructure.BusinessObjects.RunningProgram;
using RunningProgramEO = DevTrack.Infrastructure.Entities.RunningProgram;
using ScreenCaptureBO = DevTrack.Infrastructure.BusinessObjects.ScreenCapture;
using ScreenCaptureEO = DevTrack.Infrastructure.Entities.ScreenCapture;
using WebcamCaptureBO = DevTrack.Infrastructure.BusinessObjects.WebcamCapture;
using WebcamCaptureEO = DevTrack.Infrastructure.Entities.WebcamCapture;

namespace DevTrack.Infrastructure.Profiles
{
    public class InfrastructureProfile : Profile
    {
        public InfrastructureProfile()
        {
            CreateMap<ProjectBO, ProjectEO>()
                .ReverseMap().ForMember(x => x.ProjectApplicationUsers, y => y.
                MapFrom(x => x.ProjectApplicationUsers));

            CreateMap<ApplicationUserBO, ApplicationUserEO>().ReverseMap();
            CreateMap<ProjectUserList, ProjectUser>().ReverseMap();
            CreateMap<ActivityBO, ActivityEO>().ReverseMap();
            CreateMap<KeyboardActivityBO, KeyboardActivityEO>().ReverseMap();
            CreateMap<MouseActivityBO, MouseActivityEO>().ReverseMap();
            CreateMap<ScreenCaptureBO, ScreenCaptureEO>().ReverseMap();
            CreateMap<WebcamCaptureBO, WebcamCaptureEO>().ReverseMap();
            CreateMap<ActiveWindowBO, ActiveWindowEO>().ReverseMap();
            CreateMap<RunningProgramBO, RunningProgramEO>().ReverseMap();
            CreateMap<InvitationBO, InvitationEO>().ReverseMap();
            CreateMap<ActivityEO, ActivityBO>().ReverseMap();
            CreateMap<ScreenCaptureEO, ScreenCaptureBO>().ReverseMap();
            CreateMap<WebcamCaptureEO, WebcamCaptureBO>().ReverseMap();
            CreateMap<RunningProgramEO, RunningProgramBO>().ReverseMap();
            CreateMap<ActiveWindowEO, ActiveWindowBO>().ReverseMap();
            CreateMap<KeyboardActivityEO, KeyboardActivityBO>().ReverseMap();
            CreateMap<MouseActivityEO, MouseActivityBO>().ReverseMap();
            CreateMap<TeamMember, ApplicationUserBO>().ReverseMap();
        }
    }
}
