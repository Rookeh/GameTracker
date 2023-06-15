namespace GameTracker.Plugins.PlayStation.Interfaces
{
    public interface IAuthenticationHelper
    {
        Task<string> ExchangeCodeForToken(string code);
        Task<string> ExchangeNpssoForCode(string npsso);
    }
}