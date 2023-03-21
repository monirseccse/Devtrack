using Autofac;
using DevTrack.Infrastructure.BusinessObjects;
using DevTrack.Infrastructure.Services;
using DevTrack.Web.Areas.App.Models.Projects;

namespace DevTrack.Web.Areas.App.Models.Reports
{
    public class TrackHourModel
    {
        private ILifetimeScope _scope;
        private string _path;

        public TrackHourModel()
        {

        }

        public TrackHourModel(ILifetimeScope scope, IConfiguration configuration)
        {
            _scope = scope;
            _path = configuration.GetValue<string>("AccountImageLocationPath");
        }

        public async Task<object?> GetTrackedHoursAsync(Guid projectId)
        {
            var projectListModel = _scope.Resolve<ProjectListModel>();
            var TeamMembersDetails = await projectListModel.GetTeamMembersAsync(projectId);
            var ProjectCreatedDate = await projectListModel.GetProjectCreatedDate(projectId);
            var allTeamMemberTrackedRecords = new List<List<string>>();
            var totalTrackedHour = 0.0;

            foreach (var teamMember in TeamMembersDetails)
            {
                var model = new ReportCreateModel();
                model.ResolveDependency(_scope);
                model.ProjectId = projectId;
                model.ApplicationUserId = teamMember.MemberId;
                model.DateRange = ProjectCreatedDate.ToString("MM/dd/yyyy hh:mm:ss tt") + '-' + DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt");
                var Activities = await model.GetReportsAsync(model);
                var teamMemberTrackedHour = 0.0;

                foreach (var activity in Activities)
                {
                    TimeSpan ts = activity.EndTime - activity.StartTime;
                    var x = ts.TotalHours;
                    teamMemberTrackedHour += ts.TotalHours;
                }
                totalTrackedHour += teamMemberTrackedHour;
                var tempData = new List<string>();
                tempData.Add(teamMember.Name);
                tempData.Add(teamMember.Role);
                tempData.Add(teamMemberTrackedHour.ToString("0.00"));
                tempData.Add(_path + teamMember.Image);

                if (teamMember.Role == "Owner")
                    allTeamMemberTrackedRecords.Insert(0, tempData);
                else
                    allTeamMemberTrackedRecords.Add(tempData);
            }


            return new
            {
                data = (from record in allTeamMemberTrackedRecords
                        select new string[]
                        {
                            record[0],
                            record[1],
                            record[2],
                            totalTrackedHour.ToString("0.00"),
                            record[3]
                        }
                    ).ToArray()
            };
        }
    }
}
