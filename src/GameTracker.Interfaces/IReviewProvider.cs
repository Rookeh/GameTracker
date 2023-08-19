using GameTracker.Models;

namespace GameTracker.Interfaces
{
    public interface IReviewProvider : IDataProvider
    {
        public new IEnumerable<Game> Games { get; set; }
    }
}