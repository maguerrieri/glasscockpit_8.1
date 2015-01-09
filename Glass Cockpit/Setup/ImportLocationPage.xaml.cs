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
    public sealed partial class ImportLocationPage : Page
    {
        AppState state;

        public ImportLocationPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && e.Parameter is AppState) this.state = (AppState)e.Parameter;

            if (this.state.currentLoadFolder != null) this.forwardButton.Visibility = Visibility.Visible;
            else this.forwardButton.Visibility = Visibility.Collapsed;
        }

        private async void goBack(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                frame.Navigate(typeof(ExportLocationPage), this.state);
                Window.Current.Content = frame;
            });
        }

        private async void goForward(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                frame.Navigate(typeof(ArchiveLocationPage), this.state);
                Window.Current.Content = frame;
            });
        }

        private async void selectFolder(object sender, RoutedEventArgs e)
        {
            StorageFolder folder = await AppState.pickLoadFolder();

            if (folder != null)
            {
                state.currentLoadFolder = folder;

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    var frame = new Frame();
                    frame.Navigate(typeof(ArchiveLocationPage), state);
                    Window.Current.Content = frame;
                });
            }
        }
    }
}
