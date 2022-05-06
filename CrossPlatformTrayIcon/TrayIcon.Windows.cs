// <copyright file="TrayIcon.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossPlatformTrayIcon
{
    public partial class TrayIcon
    {
        private System.Windows.Forms.ContextMenuStrip? contextMenuStrip;
        private NotifyIcon? notifyIcon;
        private Icon? icon;

        private void SetupStatusBarButton()
        {
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.Icon = this.icon;
            this.notifyIcon.Text = this.iconName;
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += this.NotifyIcon_MouseClick;
        }

        private void NotifyIcon_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.LeftClicked?.Invoke(this, EventArgs.Empty);
            }

            if (e.Button == MouseButtons.Right)
            {
                this.RightClicked?.Invoke(this, EventArgs.Empty);
            }
        }

        private void NativeElementDispose()
        {
            this.notifyIcon?.Dispose();
            this.icon?.Dispose();
        }

        private void SetupStatusBarImage()
        {
            if (this.iconStream is not null)
            {
                this.icon = new Icon(this.iconStream);
            }
        }

        private void SetupStatusBarMenu()
        {
            if (!this.menuItems.Any())
            {
                return;
            }

            if (this.notifyIcon is null)
            {
                return;
            }

            this.contextMenuStrip = new ContextMenuStrip();
            this.contextMenuStrip.ItemClicked += this.ContextMenuStrip_ItemClicked;
            var items = this.menuItems.Select(n => this.GenerateItem(n)).Reverse().ToArray();
            this.contextMenuStrip.Items.AddRange(items);
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
        }

        private void ContextMenuStrip_ItemClicked(object? sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem is DrasticToolStripMenuItem stripItem)
            {
                this.MenuClicked?.Invoke(this, new TrayMenuClickedEventArgs(stripItem.Item));
            }
        }

        private DrasticToolStripMenuItem GenerateItem(TrayMenuItem item)
        {
            var menu = new DrasticToolStripMenuItem(item);
            menu.Text = item.Text;
            if (item.Icon is not null)
            {
                menu.Image = System.Drawing.Image.FromStream(item.Icon);
            }

            return menu;
        }

        private class DrasticToolStripMenuItem : ToolStripMenuItem
        {
            public DrasticToolStripMenuItem(TrayMenuItem item)
            {
                this.Text = item.Text;
                if (item.Icon is not null)
                {
                    this.Image = System.Drawing.Image.FromStream(item.Icon);
                }

                this.Item = item;
            }

            public TrayMenuItem Item { get; }
        }
    }
}
