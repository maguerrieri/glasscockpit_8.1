using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Glass_Cockpit {
	public class NavPage : Page {
		protected AppState appState;

		public NavPage() {

		}

		protected override void OnNavigatedTo(NavigationEventArgs e) {
			if (e.Parameter is AppState) {
				this.appState = (AppState)e.Parameter;
			}
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
