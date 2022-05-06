// <copyright file="MainPage.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DragAndDropOverlay;

/// <summary>
/// Main Page.
/// </summary>
public partial class MainPage : ContentPage
{
    private DragAndDropOverlay? overlay;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainPage"/> class.
    /// </summary>
    public MainPage()
    {
        this.InitializeComponent();
    }

    /// <inheritdoc/>
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (this.overlay is null && this.GetParentWindow() is App.DragAndDropWindow win)
        {
            this.overlay = win.DragAndDropOverlay;
            this.overlay.Drop += this.Overlay_Drop;
        }
    }

    private void Overlay_Drop(object? sender, DragAndDropOverlayDroppedEventArgs e)
    {
        if (e is null || !e.Paths.Any())
        {
            return;
        }

        // Get the first path.
        var path = e.Paths.First();

        if (!System.IO.File.Exists(path))
        {
            return;
        }

        try
        {
            this.DragAndDropImage.Source = ImageSource.FromFile(path);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
}