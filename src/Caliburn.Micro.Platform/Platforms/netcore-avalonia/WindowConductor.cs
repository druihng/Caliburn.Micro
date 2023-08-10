﻿using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;

namespace Caliburn.Micro
{
    public class WindowConductor
    {
        private bool deactivatingFromView;
        private bool deactivateFromViewModel;
        private bool actuallyClosing;
        private readonly Window view;
        private readonly object model;

        public WindowConductor(object model, Window view)
        {
            this.model = model;
            this.view = view;
        }

        public async Task InitialiseAsync()
        {
            if (model is IActivate activator)
            {
                await activator.ActivateAsync();
            }

            if (model is IDeactivate deactivatable)
            {
                view.Closed += Closed;
                deactivatable.Deactivated += Deactivated;
            }

            if (model is IGuardClose guard)
            {
                view.Closing += Closing;
            }
        }

        private async void Closed(object sender, EventArgs e)
        {
            view.Closed -= Closed;
            view.Closing -= Closing;

            if (deactivateFromViewModel)
            {
                return;
            }

            var deactivatable = (IDeactivate)model;

            deactivatingFromView = true;
            await deactivatable.DeactivateAsync(true);
            deactivatingFromView = false;
        }

        private Task Deactivated(object sender, DeactivationEventArgs e)
        {
            if (!e.WasClosed)
            {
                return Task.FromResult(false);
            }

            ((IDeactivate)model).Deactivated -= Deactivated;

            if (deactivatingFromView)
            {
                return Task.FromResult(true);
            }

            deactivateFromViewModel = true;
            actuallyClosing = true;
            view.Close();
            actuallyClosing = false;
            deactivateFromViewModel = false;

            return Task.FromResult(true);
        }

        private void Closing(object sender, CancelEventArgs e)
        {
            if (e.Cancel)
            {
                return;
            }

            if (actuallyClosing)
            {
                actuallyClosing = false;
                return;
            }

            e.Cancel = true;

            _ = Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var canClose = await ((IGuardClose)model).CanCloseAsync(CancellationToken.None);

                if (!canClose)
                    return;

                actuallyClosing = true;
                view.Close();
            // On macOS a crash occurs when view.Close() is called after a suspension with DispatcherPriority higher than Input.
            }, DispatcherPriority.Input);
        }
    }
}
