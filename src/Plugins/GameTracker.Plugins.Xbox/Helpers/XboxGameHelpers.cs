using GameTracker.Models.Constants;
using GameTracker.Models;
using System.Text.RegularExpressions;
using GameTracker.Models.Enums;

namespace GameTracker.Plugins.Xbox.Helpers
{
    internal static class XboxGameHelpers
    {
        internal static IEnumerable<ControlScheme> GetControlSchemesFromDevices(string[] devices)
        {
            foreach (var device in devices)
            {
                switch(device)
                {
                    case Constants.Devices.PC:
                    case Constants.Devices.Win32:
                        yield return ControlScheme.KeyboardMouse; 
                        break;
                    case Constants.Devices.Xbox360:
                    case Constants.Devices.XboxOne:
                    case Constants.Devices.XboxSeries:
                        yield return ControlScheme.Controller;
                        break;
                }
            }
        }

        internal static Image GetImageFromUrl(string url)
        {
            return new Image
            {
                Url = url,
                Width = 310,
                Height = 310
            };
        }

        internal static LaunchCommand GetLaunchCommand(string pfn)
        {
            return new LaunchCommand
            {
                Icon = "Xbox",
                NewTab = false,
                Text = "Launch via Microsoft Store",
                Url = $"ms-windows-store://pdp/?PFN={pfn}"
            };
        }

        internal static IEnumerable<Platform> GetPlatforms(string[] devices)
        {
            foreach (var device in devices)
            {
                switch (device)
                {
                    case Constants.Devices.PC:
                    case Constants.Devices.Win32:
                        yield return WellKnownPlatforms.Windows;
                        break;
                    case Constants.Devices.Xbox360:
                        yield return Constants.ConsolePlatforms.Xbox360;
                        break;
                    case Constants.Devices.XboxOne:
                        yield return Constants.ConsolePlatforms.XboxOne;
                        break;
                    case Constants.Devices.XboxSeries:
                        yield return Constants.ConsolePlatforms.XboxSeries;
                        break;
                    default: yield break;
                }
            }
        }

        internal static Publisher? GetPublisher(string pfn)
        {
            if (string.IsNullOrEmpty(pfn) || Regex.IsMatch(pfn, "^\\d"))
            {
                return null;
            }

            var publisherName = "Unknown";

            var pfnArr = pfn.Split('.');
            if (pfnArr.Any())
            {
                publisherName = Regex.Replace(pfnArr[0], "([a-z](?=[A-Z]|[0-9])|[A-Z](?=[A-Z][a-z]|[0-9])|[0-9](?=[^0-9]))", "$1 ");
            }

            return new Publisher
            {
                Name = publisherName
            };
        }
    }
}