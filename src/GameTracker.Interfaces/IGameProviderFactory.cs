namespace GameTracker.Interfaces
{
    public interface IGameProviderFactory
    {
        IEnumerable<IGameProvider> GetProviders();
    }
}