// <copyright file="MauiTrayWindow.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using CoreAnimation;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Handlers;
using ObjCRuntime;
using UIKit;

namespace TrayWindow
{
    /// <summary>
    /// TODO: This doesn't fully work yet.
    /// </summary>
    public partial class MauiTrayWindow
    {
        private UIKit.UIWindow? uiWindow;
        private NSObject? nsWindow;
        private bool isActivated;
        private TrayUIViewController? viewController;

        /// <inheritdoc/>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            this.SetupWindow();
            this.SetupTrayIcon();
        }

        private async void SetupWindow()
        {
            var handler = this.Handler as Microsoft.Maui.Handlers.WindowHandler;
            if (handler?.PlatformView is not UIKit.UIWindow uiWindow)
            {
                return;
            }

            this.uiWindow = uiWindow;

            this.nsWindow = await this.uiWindow.GetNSWindowFromUIWindow();

            await this.uiWindow.ToggleTitleBarButtons(true);

            if (this.uiWindow.RootViewController is null)
            {
                return;
            }

            this.uiWindow.RootViewController = this.viewController = new TrayUIViewController(this.uiWindow, this.uiWindow.RootViewController, this.icon, this.options);
        }

        private void ShowWindow()
        {
            this.viewController?.ToggleVisibility();
        }

        private void HideWindow()
        {
            this.viewController?.ToggleVisibility();
        }
    }
}
