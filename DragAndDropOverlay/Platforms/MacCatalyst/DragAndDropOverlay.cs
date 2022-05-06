// <copyright file="DragAndDropOverlay.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using CoreGraphics;
using Foundation;
using Microsoft.Maui.Platform;
using UIKit;

namespace DragAndDropOverlay
{
    /// <summary>
    /// Drag And Drop Overlay.
    /// </summary>
    public partial class DragAndDropOverlay
    {
        private DragAndDropView? dragAndDropView;

        /// <inheritdoc/>
        public override bool Initialize()
        {
            if (this.dragAndDropOverlayPlatformElementsInitialized)
            {
                return true;
            }

            base.Initialize();

            if (this.Window?.Handler?.MauiContext is null)
            {
                return false;
            }

            var nativeLayer = this.Window.ToPlatform(this.Window.Handler.MauiContext);
            if (nativeLayer is not UIWindow nativeWindow)
            {
                return false;
            }

            if (nativeWindow?.RootViewController?.View == null)
            {
                return false;
            }

            // We're going to create a new view.
            // This will handle the "drop" events, and nothing else.
            this.dragAndDropView = new DragAndDropView(this, nativeWindow.RootViewController.View.Frame);
            this.dragAndDropView.UserInteractionEnabled = true;
            nativeWindow?.RootViewController.View.AddSubview(this.dragAndDropView);
            nativeWindow?.RootViewController.View.BringSubviewToFront(this.dragAndDropView);
            return this.dragAndDropOverlayPlatformElementsInitialized = true;
        }

        /// <inheritdoc/>
        public override bool Deinitialize()
        {
            if (this.dragAndDropView != null)
            {
                this.dragAndDropView.RemoveFromSuperview();
                this.dragAndDropView.Dispose();
            }

            return base.Deinitialize();
        }

        private class DragAndDropView : UIView, IUIDropInteractionDelegate
        {
            private readonly DragAndDropOverlay overlay;

            public DragAndDropView(DragAndDropOverlay overlay, CGRect frame)
                : base(frame)
            {
                this.overlay = overlay;
                this.AddInteraction(new UIDropInteraction(this) { AllowsSimultaneousDropSessions = true });
            }

            [Export("dropInteraction:canHandleSession:")]
            public bool CanHandleSession(UIDropInteraction interaction, IUIDropSession session)
            {
                return true;
            }

            [Export("dropInteraction:sessionDidEnter:")]
            public void SessionDidEnter(UIDropInteraction interaction, IUIDropSession session)
            {
                this.overlay.IsDragging = true;
            }

            [Export("dropInteraction:sessionDidExit:")]
            public void SessionDidExit(UIDropInteraction interaction, IUIDropSession session)
            {
                this.overlay.IsDragging = false;
            }

            [Export("dropInteraction:sessionDidUpdate:")]
            public UIDropProposal SessionDidUpdate(UIDropInteraction interaction, IUIDropSession session)
            {
                if (session.LocalDragSession == null)
                {
                    return new UIDropProposal(UIDropOperation.Copy);
                }

                return new UIDropProposal(UIDropOperation.Cancel);
            }

            [Export("dropInteraction:performDrop:")]
            public async void PerformDrop(UIDropInteraction interaction, IUIDropSession session)
            {
                session.ProgressIndicatorStyle = UIDropSessionProgressIndicatorStyle.None;
                var filePaths = new List<string>();
                foreach (UIDragItem item in session.Items)
                {
                    var result = await this.LoadItemAsync(item.ItemProvider, item.ItemProvider.RegisteredTypeIdentifiers.ToList());
                    if (result is not null)
                    {
                        filePaths.Add(result.FileUrl.Path);
                    }
                }

                this.overlay.Drop?.Invoke(this, new DragAndDropOverlayDroppedEventArgs(filePaths));
            }

            [Export("dropInteraction:concludeDrop:")]
            public void ConcludeDrop(UIDropInteraction interaction, IUIDropSession session)
            {
                this.overlay.IsDragging = false;
            }

            public override bool PointInside(CGPoint point, UIEvent? uievent)
            {
                // Event 9 is the combination drag and drop event.
                if (uievent != null && (long)uievent.Type == 9)
                {
                    return true;
                }

                return false;
            }

            private async Task<LoadInPlaceResult?> LoadItemAsync(NSItemProvider itemProvider, List<string> typeIdentifiers)
            {
                if (typeIdentifiers is null || !typeIdentifiers.Any())
                {
                    return null;
                }

                var typeIdent = typeIdentifiers.First();

                if (itemProvider.HasItemConformingTo(typeIdent))
                {
                    return await itemProvider.LoadInPlaceFileRepresentationAsync(typeIdent);
                }

                typeIdentifiers.Remove(typeIdent);

                return await this.LoadItemAsync(itemProvider, typeIdentifiers);
            }
        }
    }
}
