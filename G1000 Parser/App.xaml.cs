using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Glass_Cockpit
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (ApplicationData.Current.RoamingSettings.Values["setup"] == null)
            {
                bool loadState = (args.PreviousExecutionState == ApplicationExecutionState.Terminated);
                Setup.ExtendedSplash extendedSplash = new Setup.ExtendedSplash(args.SplashScreen, loadState);
                Window.Current.Content = extendedSplash;
            }
            else
            {
                Frame rootFrame = Window.Current.Content as Frame;

                if (rootFrame == null) // Do not repeat app initialization when the Window already has content, just ensure that the window is active
                {
                    rootFrame = new Frame(); // Create a Frame to act as the navigation context and navigate to the first page

                    Window.Current.Content = rootFrame; // Place the frame in the current Window
                }

                if (rootFrame.Content == null)
                {
                    if (!rootFrame.Navigate(typeof(MainPage))) // When the navigation stack isn't restored navigate to the first page, configuring the new page by passing required information as a navigation parameter
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }

                Window.Current.Activate(); // Ensure the current window is active

                //this.init();
            }
        }

        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            if (ApplicationData.Current.RoamingSettings.Values["setup"] == null)
            {
                bool loadState = (args.PreviousExecutionState == ApplicationExecutionState.Terminated);
                Setup.ExtendedSplash extendedSplash = new Setup.ExtendedSplash(args.SplashScreen, loadState);
                Window.Current.Content = extendedSplash;
            }
            else
            {
                Frame rootFrame = Window.Current.Content as Frame;

                if (rootFrame == null) // Do not repeat app initialization when the Window already has content, just ensure that the window is active
                {
                    rootFrame = new Frame(); // Create a Frame to act as the navigation context and navigate to the first page

                    Window.Current.Content = rootFrame; // Place the frame in the current Window
                }

                if (rootFrame.Content == null)
                {
                    if (!rootFrame.Navigate(typeof(MainPage))) // When the navigation stack isn't restored navigate to the first page, configuring the new page by passing required information as a navigation parameter
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }

                Window.Current.Activate(); // Ensure the current window is active

                //this.init();
            }
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            
            deferral.Complete();
        }
    }
}
