using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Glass_Cockpit {
	public sealed partial class NavBar : AppBar {
		public NavBar() {
			this.InitializeComponent();
		}

		public event RoutedEventHandler Home_Click;
		public event RoutedEventHandler Errors_Click;
		public event RoutedEventHandler Files_Click;

		private void homeClicked(object sender, RoutedEventArgs e) {
			this.Home_Click.Invoke(sender, e);
		}

		private void errorsClicked(object sender, RoutedEventArgs e) {
			this.Errors_Click.Invoke(sender, e);
		}

		private void filesClicked(object sender, RoutedEventArgs e) {
			this.Files_Click.Invoke(sender, e);
		}
	}
}
