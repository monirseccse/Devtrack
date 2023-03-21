using DevTrack.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevTrack.Infrastructure.DbContexts
{
    public interface IApplicationDbContext
    {
        DbSet<ActiveWindow> ActiveWindows { get; set; }
        DbSet<Activity> Activities { get; set; }
        DbSet<ApplicationUser> ApplicationUsers { get; set; }
        DbSet<Invitation> Invitations { get; set; }
        DbSet<KeyboardActivity> KeyboardActivities { get; set; }
        DbSet<MouseActivity> MouseActivities { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<ProjectUser> ProjectUsers { get; set; }
        DbSet<RunningProgram> RunningPrograms { get; set; }
        DbSet<ScreenCapture> ScreenCaptures { get; set; }
        DbSet<WebcamCapture> WebcamCaptures { get; set; }
    }
}