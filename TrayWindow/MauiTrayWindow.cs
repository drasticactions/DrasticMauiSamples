// <copyright file="MauiTrayWindow.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using CrossPlatformTrayIcon;

namespace TrayWindow
{
    /// <summary>
    /// Maui Tray Window.
    /// </summary>
    public partial class MauiTrayWindow : Window
    {
        private TrayIcon icon;
        private TrayWindowOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="MauiTrayWindow"/> class.
        /// </summary>
        /// <param name="icon"><see cref="TrayIcon"/>.</param>
        /// <param name="options"><see cref="TrayWindowOptions"/>.</param>
        public MauiTrayWindow(TrayIcon icon, TrayWindowOptions? options = null)
        {
            this.icon = icon;
            this.options = options ?? new TrayWindowOptions();
        }

        /// <summary>
        /// Gets a value indicating whether the tray window should be visible.
        /// </summary>
        public bool IsVisible { get; private set; }

        private void SetupTrayIcon()
        {
            if (this.icon is null)
            {
                return;
            }

            this.icon.LeftClicked += this.Icon_LeftClicked;
        }

        private void Icon_LeftClicked(object? sender, EventArgs e)
        {
            if (this.IsVisible)
            {
                this.HideWindow();
            }
            else
            {
                this.ShowWindow();
            }

            this.IsVisible = !this.IsVisible;
        }
    }
}
