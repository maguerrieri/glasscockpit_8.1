using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Glass_Cockpit
{
    public sealed partial class DataSettingsFlyout : SettingsFlyout
    {
        private AppState state;

        public DataSettingsFlyout(AppState state)
        {
            this.InitializeComponent();

            this.state = state;
        }

        private void MySettingsBackClicked(object sender, RoutedEventArgs e)
        {
            if (this.Parent.GetType() == typeof(Popup))
            {
                ((Popup)this.Parent).IsOpen = false;
            }
            SettingsPane.Show();
        }

        private async void ChangeSaveLocation(object sender, RoutedEventArgs e)
        {
            this.state.currentSaveFile = await AppState.pickSaveFile();
        }

        private async void ChangeLoadLocation(object sender, RoutedEventArgs e)
        {
            this.state.currentLoadFolder = await AppState.pickLoadFolder();
        }

        private async void ChangeArchiveLocation(object sender, RoutedEventArgs e)
        {
            this.state.currentArchiveFolder = await AppState.pickArchiveFolder();
        }

        private void AutoLoadToggled(object sender, RoutedEventArgs e)
        {
            this.state.autoLoad = this.AutoLoadToggle.IsOn;
        }

        private void onLoaded(object sender, RoutedEventArgs e)
        {
            this.AutoLoadToggle.IsOn = this.state.autoLoad;
            this.saveLocationBlock.DataContext = this.state.currentSaveFile.Name;
            this.loadLocationBlock.DataContext = this.state.currentLoadFolder.Name;
            this.archiveLocationBlock.DataContext = this.state.currentArchiveFolder.Name;
        }

        private void onUnLoaded(object sender, RoutedEventArgs e)
        {
            this.state.saveState();
        }
    }
}