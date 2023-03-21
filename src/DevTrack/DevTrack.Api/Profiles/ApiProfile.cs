using AutoMapper;
using DevTrack.Api.Model;
using DevTrack.Infrastructure.BusinessObjects;
using ActiveWindow = DevTrack.Api.Model.ActiveWindow;
using ActiveWindowBO = DevTrack.Infrastructure.BusinessObjects.ActiveWindow;
using KeyboardActivity = DevTrack.Api.Model.KeyboardActivity;
using KeyboardActivityBO = DevTrack.Infrastructure.BusinessObjects.KeyboardActivity;
using MouseActivity = DevTrack.Api.Model.MouseActivity;
using MouseActivityBO = DevTrack.Infrastructure.BusinessObjects.MouseActivity;
using ProjectBO = DevTrack.Infrastructure.BusinessObjects.Project;
using RunningProgram = DevTrack.Api.Model.RunningProgram;
using RunningProgramBO = DevTrack.Infrastructure.BusinessObjects.RunningProgram;
using ScreenCapture = DevTrack.Api.Model.ScreenCapture;
using ScreenCaptureBO = DevTrack.Infrastructure.BusinessObjects.ScreenCapture;
using WebcamCapture = DevTrack.Api.Model.WebcamCapture;
using WebcamCaptureBO = DevTrack.Infrastructure.BusinessObjects.WebcamCapture;

namespace FirstDemo.API.Profiles
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {    
            CreateMap<ProjectModel, ProjectBO>().ReverseMap();
            CreateMap<ScreenCapture, ScreenCaptureBO>().ReverseMap();
            CreateMap<WebcamCapture, WebcamCaptureBO>().ReverseMap();
            CreateMap<RunningProgram, RunningProgramBO>().ReverseMap();
            CreateMap<ActiveWindow, ActiveWindowBO>().ReverseMap();
            CreateMap<KeyboardActivity, KeyboardActivityBO>().ReverseMap();
            CreateMap<MouseActivity, MouseActivityBO>().ReverseMap();
            CreateMap<ActivityDataModel, Activity>()
              .ForMember(src => src.RunningPrograms, dest => dest.MapFrom(x => x.RunningProgram))
              .ForMember(src => src.Id, dest => dest.MapFrom(x => x.ActivityId))
              .ForMember(src => src.ApplicationUserId, dest => dest.MapFrom(x => x.UserId))
              .ReverseMap();
        }
    }
}
