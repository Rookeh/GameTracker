using GameTracker.Plugins.GOG.Models.GOGApi;

namespace GameTracker.Plugins.GOG.Interfaces
{
    public interface IAuthenticationHelper
    {
        Task<AuthToken> ExchangeGogAuthCodeForToken(string code);
    }
}