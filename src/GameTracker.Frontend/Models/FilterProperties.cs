namespace GameTracker.Frontend.Models
{
    public class FilterProperties
    {
        public string Name { get; set; }
        public DateOnly? Date { get; set; }
        public int? UpperHours { get; set; }
        public int? LowerHours { get; set; }
        public int? MinimumReviewScore { get; set; }
    }
}