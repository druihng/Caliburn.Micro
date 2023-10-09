﻿using System;

namespace Caliburn.Micro;

/// <summary>
/// EventArgs sent during deactivation.
/// </summary>
public class DeactivationEventArgs : EventArgs {
    /// <summary>
    /// Gets or sets a value indicating whether the sender was closed in addition to being deactivated.
    /// </summary>
    public bool WasClosed { get; set; }
}
