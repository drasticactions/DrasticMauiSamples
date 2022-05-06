// <copyright file="DragAndDropOverlayDroppedEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace DragAndDropOverlay
{
    /// <summary>
    /// Drag and Drop Overlay Tapped Event Args.
    /// </summary>
    public class DragAndDropOverlayDroppedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DragAndDropOverlayDroppedEventArgs"/> class.
        /// </summary>
        /// <param name="paths">Paths to files that were dropped.</param>
        public DragAndDropOverlayDroppedEventArgs(List<string> paths)
        {
            this.Paths = paths;
        }

        /// <summary>
        /// Gets the paths to files that were dropped.
        /// </summary>
        public IReadOnlyList<string> Paths { get; private set; }
    }
}
