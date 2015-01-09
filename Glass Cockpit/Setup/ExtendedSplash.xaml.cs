using System;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Glass_Cockpit.Setup {
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class ExtendedSplash : Page {
		internal Rect splashImageRect; // Rect to store splash screen image coordinates.
		private SplashScreen splash; // Variable to hold the splash screen object.
		internal bool dismissed = false; // Variable to track splash screen dismissal status.
		internal Frame rootFrame;

		public ExtendedSplash(SplashScreen splashscreen, bool loadState) {
			InitializeComponent();

			// Listen for window resize events to reposition the extended splash screen image accordingly.
			// This is important to ensure that the extended splash screen is formatted properly in response to snapping, unsnapping, rotation, etc...
			Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);

			splash = splashscreen;
			if (splash != null) {
				// Register an event handler to be executed when the splash screen has been dismissed.
				splash.Dismissed += new TypedEventHandler<SplashScreen, Object>(DismissedEventHandler);

				// Retrieve the window coordinates of the splash screen image.
				splashImageRect = splash.ImageLocation;
				PositionImage();
			}

			// Create a Frame to act as the navigation context 
			rootFrame = new Frame();
		}

		void PositionImage() {
			this.splashImage.SetValue(Canvas.LeftProperty, splashImageRect.X);
			this.splashImage.SetValue(Canvas.TopProperty, splashImageRect.Y);
			this.splashImage.Height = splashImageRect.Height;
			this.splashImage.Width = splashImageRect.Width;
		}

		async void DismissedEventHandler(SplashScreen sender, object e) {
			dismissed = true;

			await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
				var frame = new Frame();
				frame.Navigate(typeof(WelcomePage));
				Window.Current.Content = frame;
			});
		}

		void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e) {
			// Safely update the extended splash screen image coordinates. This function will be executed in response to snapping, unsnapping, rotation, etc...
			if (splash != null) {
				// Update the coordinates of the splash screen image.
				splashImageRect = splash.ImageLocation;
				PositionImage();
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e) {
			// Retrieve splash screen object
			splash = (SplashScreen)(e.Parameter);
			if (splash != null) {
				// Register an event handler to be executed when the splash screen has been dismissed.
				splash.Dismissed += new TypedEventHandler<SplashScreen, object>(DismissedEventHandler);

				// Retrieve the window coordinates of the splash screen image.
				splashImageRect = splash.ImageLocation;
				PositionImage();
			}
		}

		private DispatcherTimer showWindowTimer;
		private void OnShowWindowTimer(object sender, object e) {
			showWindowTimer.Stop();

			// Activate/show the window, now that the splash image has rendered
			Window.Current.Activate();
		}

		private void imageOpened(object sender, RoutedEventArgs e) {
			// ImageOpened means the file has been read, but the image hasn't been painted yet.
			// Start a short timer to give the image a chance to render, before showing the window
			// and starting the animation.
			showWindowTimer = new DispatcherTimer();
			showWindowTimer.Interval = TimeSpan.FromMilliseconds(50);
			showWindowTimer.Tick += OnShowWindowTimer;
			showWindowTimer.Start();
		}
	}
}
