namespace DevTrack.Api.Model
{
    public class Data
    {
        public Guid userId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string token { get; set; }
        public DateTime expireDate { get; set; }        
    }
}