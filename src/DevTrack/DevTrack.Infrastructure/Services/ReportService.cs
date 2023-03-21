using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.UnitOfWorks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DevTrack.Infrastructure.Services
{
    public class ReportService:IReportService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly IMapper _mapper;        
        private readonly string _path;

        public ReportService(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
            _mapper = mapper;            
            _path = configuration.GetValue<string>("ImageLocationPath");
        }

        public async Task<IList<Activity>> GetActivities(Activity activity)
        {
            var activities=await _applicationUnitOfWork.Reports.GetActivities(activity);

            IList<Activity> activitiesList = new List<Activity>();

            foreach (var item in activities)
            {
                if(item.ScreenCapture != null)
                    item.ScreenCapture.Image = _path + item.ScreenCapture.Image;

                if (item.WebcamCapture != null)
                    item.WebcamCapture.Image = _path + item.WebcamCapture.Image;

                activitiesList.Add(_mapper.Map<Activity>(item));
            }

            return activitiesList;
        }
    }
}
