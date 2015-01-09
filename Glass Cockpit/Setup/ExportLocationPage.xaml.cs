using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Glass_Cockpit.Setup
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExportLocationPage : Page
    {
        AppState state;

        public ExportLocationPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && e.Parameter is AppState) this.state = (AppState)e.Parameter;
            else this.state = new AppState();

            if (this.state.currentSaveFile != null) this.forwardButton.Visibility = Visibility.Visible;
            else this.forwardButton.Visibility = Visibility.Collapsed;
        }

        private async void goBack(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                frame.Navigate(typeof(WelcomePage), "0");
                Window.Current.Content = frame;
            });
        }

        private async void goForward(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                frame.Navigate(typeof(ImportLocationPage), this.state);
                Window.Current.Content = frame;
            });
        }

        private async void selectFile(object sender, RoutedEventArgs e)
        {
            StorageFile file = await AppState.pickSaveFile();

            if (file != null)
            {
                this.state.currentSaveFile = file;

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    var frame = new Frame();
                    frame.Navigate(typeof(ImportLocationPage), this.state);
                    Window.Current.Content = frame;
                });
            }
        }
    }
}
