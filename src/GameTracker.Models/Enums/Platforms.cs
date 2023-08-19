namespace GameTracker.Models.Enums
{
    [Flags]
    public enum Platforms
    {
        None = 0,
        Windows = 1,
        MacOS = 2,
        Linux = 4,
        NintendoSwitch = 8,
        PlayStation3 = 16,
        PlayStation4 = 32,
        PlayStation5 = 64,
        Xbox360 = 128,
        XboxOne = 256,
        XboxSeries = 512
    }
}