namespace GameTracker.Models.Constants
{
    public static class DefaultValues
    {
        public static ParameterCache ParameterCache => new ParameterCache
        {
            Parameters = Array.Empty<object>(),
            ProviderId = Guid.Empty,
            UserId = string.Empty
        };
    }
}