namespace MicaAcrylicWindows;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }

    private void OnAcrylicWindow(object sender, EventArgs e)
    {
#if WINDOWS
        var window = new AcrylicWindow() { Page = new AppShell() };
        Application.Current?.OpenWindow(window);
#else

#endif
    }

    private void OnMicaWindow(object sender, EventArgs e)
    {
#if WINDOWS
        var window = new MicaWindow() { Page = new AppShell() };
        Application.Current?.OpenWindow(window);
#else

#endif
    }
}
