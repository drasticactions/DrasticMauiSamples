using System.Reflection;

namespace TrayWindow;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }
#if WINDOWS || MACCATALYST
    private MauiTrayWindow trayWindow;
#endif

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }

    private void OnTrayWindow(object sender, EventArgs e)
    {
#if WINDOWS || MACCATALYST
        if (this.trayWindow is null)
        {
            var icon = GetResourceFileContent("Icon.favicon.ico");
            var menuItems = new List<CrossPlatformTrayIcon.TrayMenuItem>
            {
                new CrossPlatformTrayIcon.TrayMenuItem("Test", action: async () => this.ShowMessage("Test Message One")),
                new CrossPlatformTrayIcon.TrayMenuItem("Test 2", action: async () => this.ShowMessage("Test Message Two")),
            };
            var trayIcon = new CrossPlatformTrayIcon.TrayIcon("MauiSample", icon, menuItems);
            this.trayWindow = new MauiTrayWindow(trayIcon) { Page = new MainPage() };
            Application.Current?.OpenWindow(this.trayWindow);
        }
#endif
    }
    private async void ShowMessage(string message)
    {
        await this.DisplayAlert("Message", message, "Okay");
    }

    private static Stream? GetResourceFileContent(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "TrayWindow." + fileName;
        if (assembly is null)
        {
            return null;
        }

        return assembly.GetManifestResourceStream(resourceName);
    }
}