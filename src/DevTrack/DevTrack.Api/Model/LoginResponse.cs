namespace DevTrack.Api.Model
{
    public class LoginResponse
    {
        public bool isSuccess { get; set; }
        public int statusCode { get; set; }
        public Data data { get; set; }
        public List<string> errors { get; set; }
    }
}
