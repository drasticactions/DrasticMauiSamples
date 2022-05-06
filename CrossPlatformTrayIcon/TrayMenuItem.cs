// <copyright file="TrayMenuItem.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace CrossPlatformTrayIcon
{
    /// <summary>
    /// Tray Menu Item.
    /// </summary>
    public class TrayMenuItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrayMenuItem"/> class.
        /// </summary>
        /// <param name="text">Menu Text.</param>
        /// <param name="icon">Icon.</param>
        /// <param name="action">Action to perform when clicked.</param>
        public TrayMenuItem(string text, Stream? icon = null, Func<Task>? action = null)
        {
            this.Text = text;
            this.Icon = icon;
            this.Action = action;
        }

        /// <summary>
        /// Gets the text for the menu item.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the icon for the menu item.
        /// Optional.
        /// </summary>
        public Stream? Icon { get; }

        /// <summary>
        /// Gets the action to be performed when the item is clicked.
        /// Optional.
        /// </summary>
        public Func<Task>? Action { get; }
    }
}
