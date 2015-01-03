using Windows.UI.Xaml.Controls;

namespace Glass_Cockpit {
	public class NavPage : Page {
		public NavPage() {
			Loaded += NavPage_Loaded;
		}

		void NavPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
			AppBar navBar = new AppBar();
			navBar.Content = new NavBar();
			this.TopAppBar = navBar;
		}
	}
}
