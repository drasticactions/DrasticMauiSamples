// <copyright file="PlatformExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.UI;
using Microsoft.UI.Windowing;

namespace DragAndDropOverlay
{
    /// <summary>
    /// Windows Platform Extensions.
    /// </summary>
    public static class PlatformExtensions
    {
        /// <summary>
        /// Checks if the window is fullscreen.
        /// </summary>
        /// <param name="iWin"><see cref="IWindow"/>.</param>
        /// <returns>True is the window is fullscreen.</returns>
        public static bool IsFullscreen(this IWindow iWin)
        {
            var handler = iWin.Handler as Microsoft.Maui.Handlers.WindowHandler;
            if (handler?.PlatformView is not Microsoft.UI.Xaml.Window window)
            {
                return false;
            }

            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            var myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            var win = AppWindow.GetFromWindowId(myWndId);

            return win.Presenter.Kind == AppWindowPresenterKind.FullScreen;
        }

        /// <summary>
        /// Toggle Full Screen Support.
        /// </summary>
        /// <param name="iWin"><see cref="IWindow"/>.</param>
        /// <param name="fullScreen">Enable Full Screen.</param>
        public static void ToggleFullScreen(this IWindow iWin, bool fullScreen)
        {
            var handler = iWin.Handler as Microsoft.Maui.Handlers.WindowHandler;
            if (handler?.PlatformView is not Microsoft.UI.Xaml.Window window)
            {
                return;
            }

            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            var myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            var win = AppWindow.GetFromWindowId(myWndId);

            if (win is not null)
            {
                if (fullScreen)
                {
                    win.SetPresenter(AppWindowPresenterKind.FullScreen);
                }
                else
                {
                    win.SetPresenter(AppWindowPresenterKind.Default);
                }
            }
        }

        public static AppWindow GetAppWindowForWinUI(this Microsoft.UI.Xaml.Window window)
        {
            var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);

            return GetAppWindowFromWindowHandle(windowHandle);
        }

        private static AppWindow GetAppWindowFromWindowHandle(IntPtr windowHandle)
        {
            var windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
            return AppWindow.GetFromWindowId(windowId);
        }
    }
}
