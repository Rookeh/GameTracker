namespace GameTracker.Frontend.Models
{
    public class FilterProperties
    {
        public string Name { get; set; }
        public string Studio { get; set; }
        public string Publisher { get; set; }
        public string Provider { get; set; }
        public string Genre { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public DateTime? LastPlayed { get; set; }
        public int? UpperHours { get; set; }
        public int? LowerHours { get; set; }
        public int? MinimumReviewScore { get; set; }
    }
}