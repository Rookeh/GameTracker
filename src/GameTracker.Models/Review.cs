namespace GameTracker.Models
{
    public class Review
    {
        public Critic Critic { get; set; }
        public Game Game { get; set; }
        public float Score { get; set; }
        public string Content { get; set; }
    }
}