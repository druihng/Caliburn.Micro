using System;
using System.Collections.Generic;
using System.Reflection;

using Android.Runtime;

namespace Caliburn.Micro.Maui {
    /// <summary>
    /// Encapsulates the app and its available services.
    /// </summary>
    public abstract class CaliburnApplication : Microsoft.Maui.MauiApplication {
        private bool isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaliburnApplication"/> class.
        /// </summary>
        /// <param name="javaReference">A <see cref="IntPtr"/> which contains the <c>java.lang.Class</c> JNI value corresponding to this type.</param>
        /// <param name="transfer">How to handle ownership.</param>
        protected CaliburnApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) {
        }

        /// <summary>
        /// Override to configure the framework and setup your IoC container.
        /// </summary>
        protected virtual void Configure() {
        }

        /// <summary>
        /// Override to tell the framework where to find assemblies to inspect for views, etc.
        /// </summary>
        /// <returns>A list of assemblies to inspect.</returns>
        protected virtual IEnumerable<Assembly> SelectAssemblies()
            => new[] { GetType().GetTypeInfo().Assembly };

        /// <summary>
        /// Called by the bootstrapper's constructor at design time to start the framework.
        /// </summary>
        protected virtual void StartDesignTime() {
            AssemblySource.Instance.Clear();
            AssemblySource.AddRange(SelectAssemblies());

            Configure();
        }

        /// <summary>
        /// Called by the bootstrapper's constructor at runtime to start the framework.
        /// </summary>B
        protected virtual void StartRuntime() {
            AssemblySourceCache.Install();
            AssemblySource.AddRange(SelectAssemblies());

            Configure();
        }

        /// <summary>
        /// Start the framework.
        /// </summary>
        protected void Initialize() {
            if (isInitialized) {
                return;
            }

            isInitialized = true;

            PlatformProvider.Current = new AndroidPlatformProvider(this);

            if (Execute.InDesignMode) {
                try {
                    StartDesignTime();
                } catch {
                    // if something fails at design-time, there's really nothing we can do...
                    isInitialized = false;
                    throw;
                }
            } else {
                StartRuntime();
            }
        }
    }
}
