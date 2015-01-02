using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Glass_Cockpit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ErrorsPage : Page
    {
        private AppState appState;

        public ErrorsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is AppState)
            {
                this.appState = (AppState)e.Parameter;
            }
        }

        private void onLoaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this.appState;
        }

        private void navigateFiles(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), this.appState);
        }

        private void navigateSaved(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ErrorsPage), this.appState);
        }
    }
}
