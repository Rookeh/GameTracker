namespace GameTracker.Interfaces
{
    public interface IProviderFactory
    {
        IEnumerable<IGameProvider> LoadProviders();
    }
}