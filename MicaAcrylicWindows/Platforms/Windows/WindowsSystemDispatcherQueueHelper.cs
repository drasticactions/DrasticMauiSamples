// <copyright file="WindowsSystemDispatcherQueueHelper.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

// Taken from https://github.com/marck7jr/winui3-systembackdrops-samples/blob/d4955f496455162f20d1b9457276b752499d2de4/src/App1/WindowsSystemDispatcherQueueHelper.cs.
using System.Runtime.InteropServices;
using Windows.System;

namespace MicaAcrylicWindows.Platforms.Windows
{
    /// <summary>
    /// Windows System Dispatcher Queue Helper.
    /// </summary>
    internal class WindowsSystemDispatcherQueueHelper
    {
        [DllImport("CoreMessaging.dll")]
        private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object? dispatcherQueueController);

        private object? m_dispatcherQueueController = null;

        [StructLayout(LayoutKind.Sequential)]
        private struct DispatcherQueueOptions
        {
            internal int dwSize;
            internal int threadType;
            internal int apartmentType;
        }

        public void EnsureWindowsSystemDispatcherQueueController()
        {
            if (DispatcherQueue.GetForCurrentThread() != null)
            {
                // one already exists, so we'll just use it.
                return;
            }

            if (this.m_dispatcherQueueController == null)
            {
                DispatcherQueueOptions options;
                options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
                options.threadType = 2;    // DQTYPE_THREAD_CURRENT
                options.apartmentType = 2; // DQTAT_COM_STA

                CreateDispatcherQueueController(options, ref this.m_dispatcherQueueController);
            }
        }
    }
}
