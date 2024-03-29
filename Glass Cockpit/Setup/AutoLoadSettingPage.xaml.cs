﻿using System;
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

namespace Glass_Cockpit.Setup {
	public sealed partial class AutoLoadSettingPage : Page {
		AppState state;

		public AutoLoadSettingPage() {
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e) {
			if (e.Parameter != null && e.Parameter is AppState)
				this.state = (AppState)e.Parameter;
		}

		private async void goBack(object sender, RoutedEventArgs e) {
			await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
				var frame = new Frame();
				frame.Navigate(typeof(ArchiveLocationPage), this.state);
				Window.Current.Content = frame;
			});
		}

		private async void goForward(object sender, RoutedEventArgs e) {
			await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
				var frame = new Frame();
				frame.Navigate(typeof(MainPage), this.state);
				Window.Current.Content = frame;
			});
		}

		private async void finishClicked(object sender, RoutedEventArgs e) {
			this.state.autoLoad = this.autoLoadToggle.IsOn;

			await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
				var frame = new Frame();
				frame.Navigate(typeof(MainPage), state);
				Window.Current.Content = frame;
			});
		}
	}
}
