using GameTracker.Plugins.Xbox.Models.OpenXBL;
using System.Net.Http.Headers;

namespace GameTracker.Plugins.Xbox.Interfaces
{
    public interface IRateLimitedXboxHttpClient
    {
        Task<XboxLiveTitleResponse> GetFromJson(string url, XboxLiveTitleResponse defaultValue, NameValueHeaderValue[] headers = null);
    }
}