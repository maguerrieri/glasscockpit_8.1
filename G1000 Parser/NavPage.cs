using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Glass_Cockpit {
	public class NavPage : Page {
		protected AppState appState;

		public NavPage() {
			//Loaded += NavPage_Loaded;
		}

		//void NavPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
		//	AppBar navBar = new AppBar();
		//	navBar.Content = new NavBar();
		//	this.TopAppBar = navBar;
		//}

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
