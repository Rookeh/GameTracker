namespace GameTracker.Plugins.EpicGames.Tests.Helpers
{
    internal static class ReflectionHelper
    {
        internal static void SetProtectedValue<T>(this T target, string propertyName, object parameter)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(target, parameter);
            }
        }
    }
}