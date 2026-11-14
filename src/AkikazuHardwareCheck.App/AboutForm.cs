using System.ComponentModel;
using System.Diagnostics;
using AkikazuHardwareCheck.Core.Localization;

namespace AkikazuHardwareCheck.App;

internal sealed class AboutForm : Form
{
    public const string REPOSITORY = "https://github.com/akikazu-pc/offline-wifi-rescue";

    public AboutForm(Translator translator, Font font, string title)
    {
        Text = translator["ui.about"];
        Font = font;
        Icon = Ui.AppIcon;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(520, 130);
        var content = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(16), FlowDirection = FlowDirection.TopDown };
        content.Controls.Add(new Label { Text = $"{title} {typeof(AboutForm).Assembly.GetName().Version}", AutoSize = true });
        var repository = new LinkLabel { Text = REPOSITORY, AutoSize = true };
        repository.LinkClicked += (_, _) => OpenExternal(REPOSITORY);
        content.Controls.Add(repository);
        Controls.Add(content);
    }

    public static void OpenExternal(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true })?.Dispose();
        }
        catch (Win32Exception)
        {
            Clipboard.SetText(url);
        }
    }
}
