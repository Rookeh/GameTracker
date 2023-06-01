using System.Web;

namespace GameTracker.Plugins.PlayStation.Helpers
{
    internal class UriHelper
    {
        internal static Uri BuildQueryString(string baseUrl, Dictionary<string, string> queryParameters)
        {
            var uriBuilder = new UriBuilder(baseUrl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            foreach(var key in queryParameters.Keys )
            {
                query[key] = queryParameters[key];
            }

            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }

        internal static Dictionary<string, string> ExtractQueryString(string uri)
        {
            var uriBuilder = new UriBuilder(uri);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            var queryDictionary = new Dictionary<string, string>();

            foreach(var key in query.Keys )
            {
                queryDictionary[key.ToString()] = query.Get(key.ToString());
            }

            return queryDictionary;
        }
    }
}