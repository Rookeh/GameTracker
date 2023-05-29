using GameTracker.Models.BaseClasses;

namespace GameTracker.Models
{
    public class Publisher : CorporateEntity
    {
        public Game[] Games { get; set; }
    }
}