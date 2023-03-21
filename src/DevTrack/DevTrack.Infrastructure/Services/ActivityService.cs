using AutoMapper;
using DevTrack.Infrastructure.Codes;
using DevTrack.Infrastructure.Entities;
using DevTrack.Infrastructure.UnitOfWorks;
using ActivityBO = DevTrack.Infrastructure.BusinessObjects.Activity;

namespace DevTrack.Infrastructure.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public ActivityService(IApplicationUnitOfWork applicationUnitOfWork,
            IImageService imageService, IMapper mapper)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<bool> SaveActivityAsync(ActivityBO activityData)
        {
            var activityEO = _mapper.Map<Activity>(activityData);

            activityEO.ScreenCapture.Image = await _imageService.SaveImageAsync(activityEO.ScreenCapture.Image,
                ImageType.ScreenCapture);

            activityEO.WebcamCapture.Image = await _imageService.SaveImageAsync(activityEO.WebcamCapture.Image,
                ImageType.WebcamCapture);

            await _applicationUnitOfWork.Activities.AddAsync(activityEO);
            await _applicationUnitOfWork.SaveAsync();

            return true;
        }
    }
}
