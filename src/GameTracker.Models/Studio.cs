using GameTracker.Models.BaseClasses;

namespace GameTracker.Models
{
    public class Studio : CorporateEntity
    {
        public Game[] Games { get; set; }
    }
}