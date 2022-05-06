// <copyright file="PlatformExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Numerics;
using System.Runtime.InteropServices;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Platform;
using ObjCRuntime;
using UIKit;

namespace TrayWindow
{
    /// <summary>
    /// Mac Platform Extensions
    /// </summary>
    public static class PlatformExtensions
    {
        public static NSObject? GetNSStatusBar()
           => Runtime.GetNSObject(Class.GetHandle("NSStatusBar"));

        /// <summary>
        /// Get NSWindow from UIWindow.
        /// </summary>
        /// <param name="window">UIWindow.</param>
        /// <returns>NSWindow as NSObject.</returns>
        public static async Task<NSObject?> GetNSWindowFromUIWindow(this UIWindow window)
        {
            if (window is null)
            {
                return null;
            }

            var nsApplication = Runtime.GetNSObject(Class.GetHandle("NSApplication"));
            if (nsApplication is null)
            {
                return null;
            }

            var sharedApplication = nsApplication.PerformSelector(new Selector("sharedApplication"));
            if (sharedApplication is null)
            {
                return null;
            }

            var applicationDelegate = sharedApplication.PerformSelector(new Selector("delegate"));
            if (applicationDelegate is null)
            {
                return null;
            }

            return await GetNSWindow(window, applicationDelegate);
        }

        public static async Task<NSObject?> GetNSWindow(UIWindow window, NSObject applicationDelegate)
        {
            var nsWindowHandle = IntPtr_objc_msgSend_IntPtr(applicationDelegate.Handle, Selector.GetHandle("hostWindowForUIWindow:"), window.Handle);
            var nsWindow = Runtime.GetNSObject<NSObject>(nsWindowHandle);
            if (nsWindow is null)
            {
                await Task.Delay(500);
                return await GetNSWindow(window, applicationDelegate);
            }

            return nsWindow;
        }

        public static async Task SetFrameForUIWindow(this UIWindow window, CGRect rect)
        {
            var nsWindow = await window.GetNSWindowFromUIWindow();
            if (nsWindow is null)
            {
                return;
            }

            var attachedWindow = nsWindow.ValueForKey(new NSString("attachedWindow"));

            if (attachedWindow is null)
            {
                return;
            }

            var windowFrame = (NSValue)attachedWindow.ValueForKey(new NSString("frame"));

            var originalOne = windowFrame.CGRectValue;

            var newRect = NSValue.FromCGRect(new CGRect(originalOne.X, originalOne.Y, originalOne.Width, originalOne.Height));

            // var point = NSValue.FromCGPoint(new CGPoint(rect.X, rect.Y));

            //void_objc_msgSend_IntPtr_bool(attachedWindow.Handle, Selector.GetHandle("setFrameTopLeftPoint:"), point.Handle, false);

            //attachedWindow.PerformSelector(new Selector("setFrame:display:"), newRect, NSNumber.FromBoolean(false));

            //void_objc_msgSend_IntPtr_bool(attachedWindow.Handle, Selector.GetHandle("setFrame:display:"), windowFrame.Handle, true);

            //void_objc_msgSend_IntPtr(attachedWindow.Handle, Selector.GetHandle("setFrameOrigin:"), point.Handle);

            windowFrame = (NSValue)attachedWindow.ValueForKey(new NSString("frame"));

            // void_objc_msgSend_IntPtr(attachedWindow.Handle, Selector.GetHandle("setFrameOrigin:"), point.Handle);

            // void_objc_msgSend_IntPtr(nsWindow.Handle, Selector.GetHandle("makeKeyAndOrderFront:"), nsWindow.Handle);

            //NSApplicationActivateIgnoringOtherApps();
        }

        public static async Task ToggleTitleBarButtons(this UIWindow window, bool hideButtons)
        {
            var nsWindow = await window.GetNSWindowFromUIWindow();
            if (nsWindow is null)
            {
                return;
            }

            var closeButton = Runtime.GetNSObject(IntPtr_objc_msgSend_nfloat(nsWindow.Handle, Selector.GetHandle("standardWindowButton:"), 0));

            if (closeButton is null)
            {
                return;
            }

            var miniaturizeButton = Runtime.GetNSObject(IntPtr_objc_msgSend_nfloat(nsWindow.Handle, Selector.GetHandle("standardWindowButton:"), 1));
            if (miniaturizeButton is null)
            {
                return;
            }

            var zoomButton = Runtime.GetNSObject(IntPtr_objc_msgSend_nfloat(nsWindow.Handle, Selector.GetHandle("standardWindowButton:"), 2));

            if (zoomButton is null)
            {
                return;
            }

            void_objc_msgSend_bool(closeButton.Handle, Selector.GetHandle("isHidden"), hideButtons);
            void_objc_msgSend_bool(miniaturizeButton.Handle, Selector.GetHandle("isHidden"), hideButtons);
            void_objc_msgSend_bool(zoomButton.Handle, Selector.GetHandle("isHidden"), hideButtons);
        }

        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
        internal static extern IntPtr IntPtr_objc_msgSend_nfloat(IntPtr receiver, IntPtr selector, NFloat arg1);

        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
        internal static extern IntPtr IntPtr_objc_msgSend_IntPtr(IntPtr receiver, IntPtr selector, IntPtr arg1);

        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
        internal static extern IntPtr IntPtr_objc_msgSend(IntPtr receiver, IntPtr selector);

        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
        internal static extern void void_objc_msgSend_IntPtr(IntPtr receiver, IntPtr selector, IntPtr arg1);

        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
        internal static extern void void_objc_msgSend_bool_IntPtr(IntPtr receiver, IntPtr selector, bool arg1, IntPtr arg2);

        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
        internal static extern void void_objc_msgSend_IntPtr_IntPtr(IntPtr receiver, IntPtr selector, IntPtr arg1, IntPtr arg2);

        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
        internal static extern void void_objc_msgSend_IntPtr_bool(IntPtr receiver, IntPtr selector, IntPtr arg1, bool arg2);

        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
        internal static extern void void_objc_msgSend_IntPtr_bool_bool(IntPtr receiver, IntPtr selector, IntPtr arg1, bool arg2, bool arg3);

        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
        internal static extern void void_objc_msgSend_bool(IntPtr receiver, IntPtr selector, bool arg1);

        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
        internal static extern void void_objc_msgSend_ulong(IntPtr receiver, IntPtr selector, ulong arg1);
    }
}
