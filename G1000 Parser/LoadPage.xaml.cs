using System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Glass_Cockpit {
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class LoadPage : Page {
		private static bool firstLoad = true;
		private AppState appState;

		public LoadPage() {
			this.InitializeComponent();
		}

		/// <summary>
		/// Invoked when this page is about to be displayed in a Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this page was reached.  The Parameter
		/// property is typically used to configure the page.</param>
		protected async override void OnNavigatedTo(NavigationEventArgs e) {
			if (e.Parameter is AppState) {
				this.appState = (AppState)e.Parameter;
			} else {
				this.appState = new AppState();
				await this.appState.loadState();
			}

			if (firstLoad) {
				SettingsPane.GetForCurrentView().CommandsRequested += MainPage_CommandsRequested;
				firstLoad = false;
			}
		}

		private void navigateErrors(object sender, RoutedEventArgs e) {
			this.Frame.Navigate(typeof(ErrorsPage), this.appState);
		}

		private void navigateSaved(object sender, RoutedEventArgs e) {
			this.Frame.Navigate(typeof(ErrorsPage), this.appState);
		}

		void MainPage_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args) {
			SettingsCommand updateSetting = new SettingsCommand("DataSettings", "Data Settings", (handler) => {
					DataSettingsFlyout settingsFlyout = new DataSettingsFlyout(this.appState);
					settingsFlyout.Show();

				});

			args.Request.ApplicationCommands.Add(updateSetting);
		}

		private void onLoaded(object sender, RoutedEventArgs e) {
			this.DataContext = this.appState;
		}

		private async void refreshClicked(object sender, RoutedEventArgs e) {
			if (!this.appState.running) {
				await this.appState.refresh();
			}
		}

		private void homeClicked(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) {

		}
	}

	public class DateConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, string language) {
			return ((DateTime)value).ToString("d");
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) { throw new NotImplementedException(); }
	}

	public class TimeConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, string language) {
			return ((DateTime)value).ToString("t");
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) { throw new NotImplementedException(); }
	}

	public class SavedConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, string language) {
			return (bool)value ? "Saved" : "Unsaved";
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language) { throw new NotImplementedException(); }
	}

	//public class StatusToColorConverter : IValueConverter
	//{
	//    public object Convert(object value, Type targetType, object parameter, string language)
	//    {
	//        if(value.Equals(""))
	//    }

	//    public object ConvertBack(object value, Type targetType, object parameter, string language) { throw new NotImplementedException(); }
	//}
}