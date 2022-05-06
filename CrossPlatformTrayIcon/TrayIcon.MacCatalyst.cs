// <copyright file="TrayIcon.MacCatalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using ObjCRuntime;

namespace CrossPlatformTrayIcon
{
    public partial class TrayIcon : NSObject
    {
        private bool setupFrames;
        private AppKit.NSImage? statusBarImage;
        private NSObject? statusBarItem;

        public CGRect GetFrame()
        {
            if (this.statusBarItem is null)
            {
                return new CGRect(0, 0, 0, 0);
            }

            var statusBarButton = Runtime.GetNSObject(PlatformExtensions.IntPtr_objc_msgSend(this.statusBarItem.Handle, Selector.GetHandle("button")));
            if (statusBarButton is null)
            {
                return new CGRect(0, 0, 0, 0);
            }

            var nsButtonWindow = Runtime.GetNSObject(PlatformExtensions.IntPtr_objc_msgSend(statusBarButton.Handle, Selector.GetHandle("window")));
            if (nsButtonWindow is null)
            {
                return new CGRect(0, 0, 0, 0);
            }

            var windowFrame = (NSValue)nsButtonWindow.ValueForKey(new NSString("frame"));

            return windowFrame.CGRectValue;
        }

        private void SetupStatusBarButton()
        {
            var statusBarObj = PlatformExtensions.GetNSStatusBar();
            if (statusBarObj is null)
            {
                return;
            }

            var systemStatusBarObj = statusBarObj.PerformSelector(new Selector("systemStatusBar"));
            this.statusBarItem = Runtime.GetNSObject(PlatformExtensions.IntPtr_objc_msgSend_nfloat(systemStatusBarObj.Handle, Selector.GetHandle("statusItemWithLength:"), -1));
            if (this.statusBarItem is null)
            {
                return;
            }

            var statusBarButton = Runtime.GetNSObject(PlatformExtensions.IntPtr_objc_msgSend(this.statusBarItem.Handle, Selector.GetHandle("button")));

            if (statusBarButton is not null && this.statusBarImage is not null)
            {
                PlatformExtensions.void_objc_msgSend_IntPtr(statusBarButton.Handle, Selector.GetHandle("setImage:"), this.statusBarImage.Handle);
                PlatformExtensions.void_objc_msgSend_bool(this.statusBarImage.Handle, Selector.GetHandle("setTemplate:"), true);
                this.statusBarImage.Size = new CoreGraphics.CGSize(32, 32);
            }

            if (statusBarButton is not null)
            {
                // Handle click
                PlatformExtensions.void_objc_msgSend_IntPtr(statusBarButton.Handle, Selector.GetHandle("setTarget:"), this.Handle);
                PlatformExtensions.void_objc_msgSend_IntPtr(statusBarButton.Handle, Selector.GetHandle("setAction:"), new Selector("handleButtonClick:").Handle);
            }
        }

        private void SetupStatusBarImage()
        {
            if (this.iconStream is null)
            {
                return;
            }

            var imageStream = Foundation.NSData.FromStream(this.iconStream);
            this.statusBarImage = Runtime.GetNSObject<AppKit.NSImage>(PlatformExtensions.IntPtr_objc_msgSend(ObjCRuntime.Class.GetHandle("NSImage"), Selector.GetHandle("alloc")));
            if (this.statusBarImage is null || imageStream is null)
            {
                throw new ArgumentNullException(nameof(this.statusBarImage));
            }

            PlatformExtensions.IntPtr_objc_msgSend_IntPtr(this.statusBarImage.Handle, Selector.GetHandle("initWithData:"), imageStream.Handle);
        }

        [Export("handleButtonClick:")]
        private void HandleClick(NSObject senderStatusBarButton)
        {
            PlatformExtensions.NSApplicationActivateIgnoringOtherApps(true);
            this.LeftClicked?.Invoke(this, EventArgs.Empty);
        }

        private void SetupStatusBarMenu()
        {
        }

        private void NativeElementDispose()
        {
            this.statusBarImage?.Dispose();
            this.statusBarItem?.Dispose();
        }
    }
}
