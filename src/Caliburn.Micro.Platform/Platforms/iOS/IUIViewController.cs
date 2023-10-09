using System;

namespace Caliburn.Micro {
    /// <summary>
    /// An interface to allow the IOSPlatformProvider provide view lifecycle events.
    /// </summary>
    public interface IUIViewController {
        /// <summary>
        /// Invoked when the view is loaded
        /// </summary>
        event EventHandler ViewLoaded;

        /// <summary>
        /// Invoked the view appears
        /// </summary>
        event EventHandler ViewAppeared;

        /// <summary>
        /// Gets a value indicating whether returns if the current view is already loaded.
        /// </summary>
        bool IsViewLoaded { get; }
    }
}
