using AutoMapper;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Web.Areas.App.Models.Projects;
using DevTrack.Web.Areas.App.Models.Reports;
using DevTrack.Web.Models;

namespace DevTrack.Web.Profiles
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<ProjectCreateModel, Project>()
                .ReverseMap();

            CreateMap<ProjectEditModel, Project>()
                .ReverseMap();

            CreateMap<ReportCreateModel, Activity>()
                .ReverseMap();
            
            CreateMap<AccountEditModel, ApplicationUser>()
                .ReverseMap();
        }
    }
}
