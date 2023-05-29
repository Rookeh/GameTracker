namespace GameTracker.Models.BaseClasses
{
    public class CorporateEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public SocialLink[] Links { get; set; }
    }
}