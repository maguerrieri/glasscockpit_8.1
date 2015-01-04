using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Glass_Cockpit {
	public sealed partial class ErrorsPage : NavPage {

		public ErrorsPage() {
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e) {
			if (e.Parameter is AppState) {
				this.appState = (AppState)e.Parameter;
			}
		}

		private void onLoaded(object sender, RoutedEventArgs e) {
			this.DataContext = this.appState;
		}

		internal void navigateHome(object sender, RoutedEventArgs e) {
			this.Frame.Navigate(typeof(MainPage), this.appState);
		}

		internal void navigateErrors(object sender, RoutedEventArgs e) {
			this.Frame.Navigate(typeof(ErrorsPage), this.appState);
		}

		internal void navigateFiles(object sender, RoutedEventArgs e) {
			this.Frame.Navigate(typeof(LoadPage), this.appState);
		}
	}
}
