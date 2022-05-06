// <copyright file="DragAndDropOverlay.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using AndroidX.CoordinatorLayout.Widget;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace DragAndDropOverlay
{
    /// <summary>
    /// Drag and Drop Overlay.
    /// </summary>
    public partial class DragAndDropOverlay
    {
        private ViewGroup? nativeLayer;
        private DragAndDropView? dragAndDropView;

        /// <inheritdoc/>
        public override bool Initialize()
        {
            if (this.dragAndDropOverlayPlatformElementsInitialized)
            {
                return true;
            }

            base.Initialize();

            var handler = this.Window?.Handler as WindowHandler;
            if (handler?.MauiContext is null)
            {
                return false;
            }

            var nativeWindow = this.Window?.Content?.ToPlatform(handler.MauiContext);
            if (nativeWindow is null)
            {
                return false;
            }

            var rootManager = handler.MauiContext.GetNavigationRootManager();
            if (rootManager is null)
            {
                return false;
            }

            if (handler.PlatformView is not Activity activity)
            {
                return false;
            }

            var context = activity.ApplicationContext;
            if (context is null)
            {
                return false;
            }

            this.nativeLayer = rootManager.RootView as ViewGroup;

            if (this.nativeLayer is null)
            {
                return false;
            }

            this.dragAndDropView = new DragAndDropView(context);
            this.nativeLayer.AddView(this.dragAndDropView, 0, new CoordinatorLayout.LayoutParams(CoordinatorLayout.LayoutParams.MatchParent, CoordinatorLayout.LayoutParams.MatchParent));
            this.dragAndDropView.BringToFront();
            return this.dragAndDropOverlayPlatformElementsInitialized = true;
        }

        private class DragAndDropView : Android.Views.View, IOnReceiveContentListener
        {
            public DragAndDropView(Context context)
                : base(context)
            {
            }

            protected DragAndDropView(IntPtr javaReference, JniHandleOwnership transfer)
                : base(javaReference, transfer)
            {
            }

            public ContentInfo? OnReceiveContent(Android.Views.View view, ContentInfo payload)
            {
                return null;
            }
        }
    }
}
