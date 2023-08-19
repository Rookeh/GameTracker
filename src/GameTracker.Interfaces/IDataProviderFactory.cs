namespace GameTracker.Interfaces
{
    public interface IDataProviderFactory
    {
        IEnumerable<IDataProvider> GetAllProviders();
        IEnumerable<IGameProvider> GetGameProviders();
        IEnumerable<IReviewProvider> GetReviewProviders();
    }
}