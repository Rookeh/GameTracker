namespace GameTracker.Models
{
    public class ParameterCache
    {
        public Guid ProviderId { get; set; }
        public string UserId { get; set; }
        public object[] Parameters { get; set; }
    }
}