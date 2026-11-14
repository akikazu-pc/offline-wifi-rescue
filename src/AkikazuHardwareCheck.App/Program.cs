using System.Globalization;
using AkikazuHardwareCheck.Core.Localization;
using AkikazuHardwareCheck.Core.SharedDesktop;

namespace AkikazuHardwareCheck.App;

internal static class Program
{
    public const string INSTALL_NETWORK_DRIVER_ARGUMENT = "--install-network-driver";

    [STAThread]
    private static void Main(string[] args)
    {
        if (args is [INSTALL_NETWORK_DRIVER_ARGUMENT, var inf, var hardwareId, var hash])
        {
            using var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            Environment.ExitCode = (int)(principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator)
                && AkikazuHardwareCheck.Core.Configuration.DistributionProfile.Current.AllowsDriverManagement
                ? OfflineDriverInstaller.Install(inf, hardwareId, hash)
                : OfflineInstallResult.InstallationFailed);
            return;
        }

        var language = args.FirstOrDefault(arg => AppLanguage.Supported.Contains(arg, StringComparer.OrdinalIgnoreCase));
        RunPortableRescue(language is null ? null : AppLanguage.Supported.First(value =>
            string.Equals(value, language, StringComparison.OrdinalIgnoreCase)));
    }

    private static void RunPortableRescue(string? languageOverride)
    {
        ApplicationConfiguration.Initialize();
        Application.SetColorMode(SystemColorMode.System);
        using var instance = new Mutex(true, @"Local\Akikazu.NetworkRescue.Portable", out var created);
        if (!created)
        {
            return;
        }

        var currentLanguage = languageOverride ?? AppLanguage.FromCulture(CultureInfo.CurrentUICulture.Name);
        var translator = Translator.Load(currentLanguage);
        using var window = new Form
        {
            Text = translator[SharedModuleId.NetworkRescue.DisplayNameKey()],
            Icon = Ui.AppIcon,
            StartPosition = FormStartPosition.CenterScreen,
            ClientSize = new Size(1100, 720),
        };
        var languages = new ToolStripDropDownButton(translator["rescue.pe.language"]);
        var rescue = new NetworkRescueView(translator, [], Path.Combine(AppContext.BaseDirectory, "driver-packs"));
        string[] languageLabels = ["日本語", "简体中文", "繁體中文", "English"];
        foreach (var (language, label) in AppLanguage.Supported.Zip(languageLabels))
        {
            var item = new ToolStripMenuItem(label) { Checked = language == currentLanguage };
            item.Click += (_, _) =>
            {
                var selected = Translator.Load(language);
                window.Text = selected[SharedModuleId.NetworkRescue.DisplayNameKey()];
                languages.Text = selected["rescue.pe.language"];
                foreach (ToolStripMenuItem option in languages.DropDownItems)
                {
                    option.Checked = option == item;
                }
                rescue.ApplyLanguage(selected);
            };
            languages.DropDownItems.Add(item);
        }
        rescue.AddPortableMenus(languages);
        window.Controls.Add(rescue);
        Application.Run(window);
    }
}
