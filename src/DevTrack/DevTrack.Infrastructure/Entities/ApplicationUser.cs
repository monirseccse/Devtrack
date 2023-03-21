using Microsoft.AspNetCore.Identity;

namespace DevTrack.Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser<Guid>, IEntity<Guid>
    {       
        public string Name { get; set; }
        public string Image { get; set; }
        public List<ProjectUser> ApplicationUserProjects { get; set; }
        public List<Activity> Activities { get; set; }
    }
}
