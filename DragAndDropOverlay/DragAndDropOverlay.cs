// <copyright file="DragAndDropOverlay.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DragAndDropOverlay
{
    /// <summary>
    /// Drag and Drop Overlay.
    /// </summary>
    public partial class DragAndDropOverlay : BaseOverlay
    {
        private readonly DropElementOverlay dropElement;
        private bool dragAndDropOverlayPlatformElementsInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="DragAndDropOverlay"/> class.
        /// </summary>
        /// <param name="window"><see cref="IWindow"/>.</param>
        /// <param name="dragOverColor">Optional color to show when draging an item over the window.</param>
        /// <param name="dragOverOverlayElement">Optional window element to show when draging an element over the window.</param>
        public DragAndDropOverlay(IWindow window, Microsoft.Maui.Graphics.Color? dragOverColor = null, IWindowOverlayElement? dragOverOverlayElement = null)
            : base(window)
        {
            this.dropElement = new DropElementOverlay();

            if (dragOverColor is not null)
            {
                this.dropElement.Color = dragOverColor;
            }

            this.dropElement.OverlayElement = dragOverOverlayElement;

            this.AddWindowElement(this.dropElement);
        }

        /// <summary>
        /// Fired when files are dropped on the overlay.
        /// </summary>
        public event EventHandler<DragAndDropOverlayDroppedEventArgs>? Drop;

        /// <summary>
        /// Gets or sets a value indicating whether a user is dragging an element over the window.
        /// </summary>
        internal bool IsDragging
        {
            get => this.dropElement.IsDragging;
            set
            {
                this.dropElement.IsDragging = value;
                this.Invalidate();
            }
        }

        private class DropElementOverlay : IWindowOverlayElement
        {
            public IWindowOverlayElement? OverlayElement { get; set; }

            public bool IsDragging { get; set; }

            public Microsoft.Maui.Graphics.Color Color { get; set; } = Microsoft.Maui.Graphics.Colors.Transparent;

            // We are not going to use Contains for this.
            // We're gonna set if it's invoked externally.
            public bool Contains(Microsoft.Maui.Graphics.Point point) => false;

            public void Draw(ICanvas canvas, Microsoft.Maui.Graphics.RectF dirtyRect)
            {
                if (!this.IsDragging)
                {
                    return;
                }

                if (this.OverlayElement is not null)
                {
                    this.OverlayElement.Draw(canvas, dirtyRect);
                    return;
                }

                canvas.FillColor = this.Color;
                canvas.FillRectangle(dirtyRect);
            }
        }
    }
}
