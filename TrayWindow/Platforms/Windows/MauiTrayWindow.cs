// <copyright file="MauiTrayWindow.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using CrossPlatformTrayIcon;
using Microsoft.UI.Windowing;
using Windows.Graphics;

namespace TrayWindow
{
    public partial class MauiTrayWindow
    {
        private Microsoft.UI.Windowing.AppWindow? appWindow;
        private bool appLaunched;

        /// <inheritdoc/>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            this.SetupWindow();
            this.SetupTrayIcon();
        }

        private void SetupWindow()
        {
            var handler = this.Handler as Microsoft.Maui.Handlers.WindowHandler;
            if (handler?.PlatformView is not Microsoft.UI.Xaml.Window window)
            {
                return;
            }

            this.appWindow = window.GetAppWindowForWinUI();
            if (this.appWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsResizable = false;
                presenter.IsAlwaysOnTop = true;
                presenter.IsMaximizable = false;
                presenter.IsMinimizable = false;
            }

            window.VisibilityChanged += this.Window_VisibilityChanged;

            this.appWindow.TitleBar.SetDragRectangles(new[] { new RectInt32(0, 0, 0, 0) });
            this.appWindow.Hide();
        }

        private void Window_VisibilityChanged(object sender, Microsoft.UI.Xaml.WindowVisibilityChangedEventArgs args)
        {
            if (!this.appLaunched)
            {
                this.appWindow?.Hide();
                this.appLaunched = true;
            }

            if (sender is Microsoft.UI.Xaml.Window window)
            {
                window.VisibilityChanged -= this.Window_VisibilityChanged;
            }
        }

        private void ShowWindow()
        {
            if (this.appWindow is null)
            {
                return;
            }

            var rectangle = CrossPlatformTrayIcon.PlatformExtensions.GetCursorPosition(this.options.WindowWidth, this.options.WindowHeight);
            this.appWindow.MoveAndResize(rectangle);
            this.appWindow.Show();
        }

        private void HideWindow()
        {
            this.appWindow?.Hide();
        }
    }
}
