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

        internal static Platforms GetPlatforms(string[] devices)
        {
            var platforms = Platforms.None;

            foreach (var device in devices)
            {
                switch (device)
                {
                    case Constants.Devices.PC:
                    case Constants.Devices.Win32:
                        platforms |= Platforms.Windows;
                        break;
                    case Constants.Devices.Xbox360:
                        platforms |= Platforms.Xbox360;
                        break;
                    case Constants.Devices.XboxOne:
                        platforms |= Platforms.XboxOne;
                        break;
                    case Constants.Devices.XboxSeries:
                        platforms |= Platforms.XboxSeries;
                        break;
                }
            }

            return platforms;
        }

        internal static string GetPublisher(string pfn)
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

            return publisherName;
        }
    }
}