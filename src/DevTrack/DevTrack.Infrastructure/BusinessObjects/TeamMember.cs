namespace DevTrack.Infrastructure.BusinessObjects
{
    public class TeamMember
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Role { get; set; }
        public Guid MemberId { get; set; }
    }
}
