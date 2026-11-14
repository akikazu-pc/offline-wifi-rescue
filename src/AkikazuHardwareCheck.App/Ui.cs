namespace AkikazuHardwareCheck.App;

internal static class Ui
{
    internal const int SPACING = 8;
    internal static Color MutedTextColor => SystemColors.GrayText;
    internal static Icon AppIcon { get; } = LoadIcon();

    internal static Label Label(bool isBold = false) => new()
    {
        AutoSize = true,
        Font = isBold ? new Font(SystemFonts.MessageBoxFont!, FontStyle.Bold) : SystemFonts.MessageBoxFont,
    };

    internal static Button Button() => new() { AutoSize = true };

    private static Icon LoadIcon()
    {
        using var stream = typeof(Ui).Assembly.GetManifestResourceStream("app.ico")!;
        return new Icon(stream);
    }
}
