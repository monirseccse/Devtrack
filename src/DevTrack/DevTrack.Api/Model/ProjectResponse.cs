namespace DevTrack.Api.Model
{
    public class ProjectResponse
    {
        public bool isSuccess { get; set; }
        public int statusCode { get; set; }
        public List<ProjectData> data { get; set; }
        public string[] errors { get; set; }        
    }
}
