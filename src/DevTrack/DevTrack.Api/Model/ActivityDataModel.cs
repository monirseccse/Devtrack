using Autofac;
using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Services;

namespace DevTrack.Api.Model
{
    public class ActivityDataModel
    {
        public Guid ActivityId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsOnline { get; set; }
        public ScreenCapture ScreenCapture { get; set; }
        public WebcamCapture WebcamCapture { get; set; }
        public KeyboardActivity KeyboardActivity { get; set; }
        public MouseActivity MouseActivity { get; set; }
        public List<RunningProgram> RunningProgram { get; set; }
        public List<ActiveWindow> ActiveWindows { get; set; }

        private IActivityService _activityService;
        private ILifetimeScope _scope;
        private IMapper _mapper;

        public ActivityDataModel()
        {

        }

        public ActivityDataModel(IActivityService activityService, ILifetimeScope scope,
            IMapper mapper)
        {
            _activityService = activityService;
            _scope = scope;
            _mapper = mapper;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _activityService = _scope.Resolve<IActivityService>();
            _mapper = _scope.Resolve<IMapper>();
        }

        internal async Task<bool> SaveActivityDataAsync()
        {
            var data = _mapper.Map<Activity>(this);
            var isSuccess = await _activityService.SaveActivityAsync(data);

            return isSuccess;
        }
    }
}
