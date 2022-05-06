// <copyright file="TrayIcon.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace CrossPlatformTrayIcon
{
    public partial class TrayIcon : IDisposable
    {
        private Stream? iconStream;
        private string? iconName;
        private List<TrayMenuItem> menuItems;
        private bool holdsWindowInTray;
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrayIcon"/> class.
        /// </summary>
        /// <param name="name">Name of the icon.</param>
        /// <param name="stream">Icon Image Stream. Optional.</param>
        /// <param name="menuItems">Items to populate context menu. Optional.</param>
        public TrayIcon(string name, Stream? stream = null, List<TrayMenuItem>? menuItems = null)
        {
            this.menuItems = menuItems ?? new List<TrayMenuItem>();
            this.iconName = name;
            this.iconStream = stream;
            this.SetupStatusBarImage();
            this.SetupStatusBarButton();
            this.SetupStatusBarMenu();
        }

        /// <summary>
        /// Left Clicked Event.
        /// </summary>
        public event EventHandler<EventArgs>? LeftClicked;

        /// <summary>
        /// Right Clicked Event.
        /// </summary>
        public event EventHandler<EventArgs>? RightClicked;

        /// <summary>
        /// Menu Item Clicked.
        /// </summary>
        public event EventHandler<TrayMenuClickedEventArgs>? MenuClicked;

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Is Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    //   this.PlatformElementDispose();
                }

                this.disposedValue = true;
            }
        }
    }
}
