using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Glass_Cockpit.Setup
{
    public sealed partial class WelcomePage : Page
    {
        private bool navigatedTo = false;

        public WelcomePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && e.Parameter.Equals("0")) this.navigatedTo = true;
        }

        private void onLoaded(object sender, RoutedEventArgs e)
        {
            if (this.navigatedTo) this.moveSplashFast.Begin();
            else this.moveSplashSlow.Begin();
        }

        private void imageTranslateCompleted(object sender, object e)
        {
            this.splashButton.Visibility = Visibility.Visible;
            this.moveButton.Begin();
        }

        private async void nextPageClicked(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                frame.Navigate(typeof(ExportLocationPage));
                Window.Current.Content = frame;
            });
        }
    }
}
