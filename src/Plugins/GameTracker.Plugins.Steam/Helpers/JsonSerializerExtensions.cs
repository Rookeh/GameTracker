using System.Text.Json;

namespace GameTracker.Plugins.Steam.Helpers
{
    public class JsonSerializerExtensions
    {
        public static T? DeserializeAnonymous<T>(string json, T anonObject, JsonSerializerOptions? options = default, CancellationToken cancellationToken = default)
            => JsonSerializer.Deserialize<T>(json, options);
    }
}