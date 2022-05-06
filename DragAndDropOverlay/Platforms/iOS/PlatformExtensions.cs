// <copyright file="PlatformExtensions.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

#pragma warning disable SA1210 // Using directives need to be in a specific order for MAUI
using System.Numerics;
using System.Runtime.InteropServices;
using Microsoft.Maui.Platform;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
#pragma warning restore SA1210 // Using directives can't be ordered alphabetically by namespace

namespace DragAndDropOverlay
{
    /// <summary>
    /// iOS Platform Extensions.
    /// </summary>
    public static class PlatformExtensions
    {
#if __MACCATALYST__
        public static NSObject? GetNSStatusBar()
          => Runtime.GetNSObject(Class.GetHandle("NSStatusBar"));

        public static void NSApplicationActivateIgnoringOtherApps(bool ignoreSetting = true)
        {
            var sharedApplication = GetNSApplicationSharedApplication();
            if (sharedApplication is null)
            {
                return;
            }

            void_objc_msgSend_bool(sharedApplication.Handle, Selector.GetHandle("activateIgnoringOtherApps:"), ignoreSetting);
        }

        public static NSObject? GetNSApplicationSharedApplication()
        {
            var nsapp = Runtime.GetNSObject(Class.GetHandle("NSApplication"));
            if (nsapp is null)
            {
                return null;
            }

            var sharedApp = nsapp.PerformSelector(new Selector("sharedApplication"));

            return null;
        }

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

        /// <summary>
        /// Gets NSWindow from UIWindow.
        /// Uses ObjectiveC selectors to get NSWindow.
        /// </summary>
        /// <param name="window"><see cref="UIWindow"/>.</param>
        /// <param name="applicationDelegate">The ApplicationDelgate. Can be from NSApplication or sharedApplication.</param>
        /// <returns>NSWindow as NSObject.</returns>
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

        /// <summary>
        /// Checks if the window is fullscreen.
        /// </summary>
        /// <param name="iWin"><see cref="IWindow"/>.</param>
        /// <returns>True is the window is fullscreen.</returns>
        public static bool IsFullscreen(this IWindow iWin)
        {
            UIWindow? window = iWin.Handler?.PlatformView as UIWindow;

            if (window is null)
            {
                return false;
            }

            return IsFullscreen(window);
        }

        /// <summary>
        /// Checks if the window is fullscreen.
        /// </summary>
        /// <param name="window"><see cref="UIWindow"/>.</param>
        /// <returns>True is the window is fullscreen.</returns>
        public static bool IsFullscreen(this UIWindow window)
        {
            var nsWindow = window.GetNSWindowFromUIWindow().Result;
            if (nsWindow is null)
            {
                return false;
            }

            var styleMaskEnum = Int_objc_msgSend(nsWindow.Handle, Selector.GetHandle("styleMask"));

            return styleMaskEnum > 0;
        }

        /// <summary>
        /// Toggle Fullscreen mode on UIWindow.
        /// </summary>
        /// <param name="window"><see cref="UIWindow"/>.</param>
        /// <param name="fullScreen">Fullscreen command.</param>
        public static void ToggleFullScreen(this UIWindow window, bool fullScreen)
        {
            var nsWindow = window.GetNSWindowFromUIWindow().Result;
            if (nsWindow is null)
            {
                return;
            }

            void_objc_msgSend_bool(nsWindow.Handle, Selector.GetHandle("toggleFullScreen:"), fullScreen);
        }

        /// <summary>
        /// Toggle Full Screen Support.
        /// </summary>
        /// <param name="win"><see cref="IWindow"/>.</param>
        /// <param name="fullScreen">Enable Full Screen.</param>
        public static void ToggleFullScreen(this IWindow win, bool fullScreen)
        {
            UIWindow? window = win.Handler?.PlatformView as UIWindow;

            if (window is null)
            {
                return;
            }

            window.ToggleFullScreen(fullScreen);
        }

        /// <summary>
        /// Send Objective-C Int Command Selector.
        /// </summary>
        /// <param name="receiver">The IntPtr Reciever.</param>
        /// <param name="selector">The IntPtr Selector.</param>
        /// <returns>IntPtr.</returns>
        [DllImport("/usr/lib/libobjc.dylib", EntryPoint = "objc_msgSend")]
        internal static extern int Int_objc_msgSend(IntPtr receiver, IntPtr selector);

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

#endif
    }
}
