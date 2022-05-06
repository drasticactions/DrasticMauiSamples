// <copyright file="DragAndDropOverlay.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Microsoft.Maui.Platform;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

namespace DragAndDropOverlay
{
    /// <summary>
    /// Drag And Drop Overlay.
    /// </summary>
    public partial class DragAndDropOverlay
    {
        private Microsoft.UI.Xaml.UIElement? panel;

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

            var nativeElement = this.Window.Content.ToPlatform(this.Window.Handler.MauiContext);
            if (nativeElement == null)
            {
                return false;
            }

            var handler = this.Window.Handler as Microsoft.Maui.Handlers.WindowHandler;
            if (handler?.PlatformView is not Microsoft.UI.Xaml.Window window)
            {
                return false;
            }

            this.panel = window.Content as Microsoft.UI.Xaml.Controls.Panel;
            if (this.panel == null)
            {
                return false;
            }

            this.panel.AllowDrop = true;
            this.panel.DragOver += this.Panel_DragOver;
            this.panel.Drop += this.Panel_Drop;
            this.panel.DragLeave += this.Panel_DragLeave;
            this.panel.DropCompleted += this.Panel_DropCompleted;
            return this.dragAndDropOverlayPlatformElementsInitialized = true;
        }

        /// <inheritdoc/>
        public override bool Deinitialize()
        {
            if (this.panel != null)
            {
                this.panel.AllowDrop = false;
                this.panel.DragOver -= this.Panel_DragOver;
                this.panel.Drop -= this.Panel_Drop;
                this.panel.DragLeave -= this.Panel_DragLeave;
                this.panel.DropCompleted -= this.Panel_DropCompleted;
            }

            return base.Deinitialize();
        }

        private void Panel_DropCompleted(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.DropCompletedEventArgs args)
        {
            this.IsDragging = false;
        }

        private void Panel_DragLeave(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            this.IsDragging = false;
        }

        private async void Panel_Drop(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Any())
                {
                    var filePaths = new List<string>();
                    foreach (var item in items)
                    {
                        if (item is StorageFile file)
                        {
                            filePaths.Add(item.Path);
                        }
                    }

                    this.Drop?.Invoke(this, new DragAndDropOverlayDroppedEventArgs(filePaths));
                }
            }
        }

        private void Panel_DragOver(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
        }
    }
}
