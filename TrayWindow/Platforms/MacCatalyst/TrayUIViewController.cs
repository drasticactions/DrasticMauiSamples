// <copyright file="TrayUIViewController.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using CoreGraphics;
using CrossPlatformTrayIcon;
using UIKit;

namespace TrayWindow
{
    /// <summary>
    /// Tray UI View Controller.
    /// Holds an existing MAUI UI View Controller and turns
    /// it into a modal popover.
    /// </summary>
    public class TrayUIViewController : UIViewController
    {
        private UIViewController contentController;
        private UIImage? image;
        private UIWindow window;
        private TrayWindowOptions options;
        private TrayIcon trayIcon;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrayUIViewController"/> class.
        /// </summary>
        /// <param name="window">Containing UI Window.</param>
        /// <param name="contentController">Embedded UI View Controller.</param>
        /// <param name="trayIcon">Tray Icon.</param>
        /// <param name="options">Options.</param>
        public TrayUIViewController(
            UIWindow window,
            UIViewController contentController,
            TrayIcon trayIcon,
            TrayWindowOptions options)
        {
            this.trayIcon = trayIcon;
            this.options = options;
            this.window = window;
            this.contentController = contentController;
            this.image = UIImage.GetSystemImage("cursorarrow.click.2");
            this.SetupWindow();
        }

        /// <summary>
        /// Toggle Visibility of the window.
        /// </summary>
        public async void ToggleVisibility()
        {
            if (this.contentController?.View is null)
            {
                return;
            }

            if (this.View is null)
            {
                return;
            }

            var buttonBounds = this.trayIcon.GetFrame();
            await this.window.SetFrameForUIWindow(buttonBounds);
            var viewController = this.contentController;

            if (viewController.PresentingViewController is not null)
            {
                viewController.DismissViewController(true, null);
            }
            else
            {
                viewController.ModalPresentationStyle = UIModalPresentationStyle.Popover;
                viewController.PopoverPresentationController.SourceView = this.View;
                viewController.PopoverPresentationController.SourceRect = new CGRect(0, 0, 1, 1);
                viewController.PopoverPresentationController.PermittedArrowDirections = UIPopoverArrowDirection.Up;
                this.PresentViewController(viewController, true, null);
            }
        }

        /// <inheritdoc/>
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            this.PrepareForAppearance();
            this.ForceContentViewLayout();
        }

        private async void PrepareForAppearance()
        {
            if (this.window is not null)
            {
                await this.window.ToggleTitleBarButtons(true);
            }
        }

        private void ForceContentViewLayout()
        {
            if (this.contentController.View is not null)
            {
                this.View?.AddSubview(this.contentController.View);
                this.contentController.View.Frame = new CoreGraphics.CGRect(0, 0, this.options.WindowWidth, this.options.WindowHeight);
                this.View?.LayoutIfNeeded();
                this.contentController.View.RemoveFromSuperview();
            }
        }

        private void SetupWindow()
        {
            this.window.RootViewController = this;
            if (this.window.WindowScene?.Titlebar is null)
            {
                return;
            }

            if (this.window.WindowScene?.SizeRestrictions is null)
            {
                return;
            }

            this.window.WindowScene.Titlebar.TitleVisibility = UITitlebarTitleVisibility.Hidden;
            this.window.WindowScene.SizeRestrictions.MinimumSize = new CoreGraphics.CGSize(1, 1);
            this.window.WindowScene.SizeRestrictions.MaximumSize = new CoreGraphics.CGSize(1, 1);
        }
    }
}
