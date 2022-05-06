// <copyright file="PlatformExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Maui.Platform;

namespace DragAndDropOverlay
{
    /// <summary>
    /// Android Platform Extensions.
    /// </summary>
    public static class PlatformExtensions
    {
        /// <summary>
        /// Gets Navigation Root Manager.
        /// </summary>
        /// <param name="mauiContext">MAUI Context.</param>
        /// <returns>NavigationRootManager.</returns>
        public static NavigationRootManager GetNavigationRootManager(this IMauiContext mauiContext) =>
            mauiContext.Services.GetRequiredService<NavigationRootManager>();
    }
}
