// <copyright file="TrayMenuClickedEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace CrossPlatformTrayIcon
{
    /// <summary>
    /// Drastic Tray Menu Clicked.
    /// </summary>
    public class TrayMenuClickedEventArgs : EventArgs
    {
        private TrayMenuItem menuItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrayMenuClickedEventArgs"/> class.
        /// </summary>
        /// <param name="item">Position.</param>
        internal TrayMenuClickedEventArgs(TrayMenuItem item)
        {
            this.menuItem = item;
        }

        /// <summary>
        /// Gets the TrayMenuItem.
        /// </summary>
        public TrayMenuItem MenuItem => this.menuItem;
    }
}
