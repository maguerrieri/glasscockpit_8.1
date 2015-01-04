using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Glass_Cockpit {
	public class AppState {
		public int filesOpened { get { return this.files.Count; } }
		public int filesSaved { get; set; }
		public int filesArchived { get; set; }

		public ObservableCollection<Flight> files { get; private set; }
		public ObservableCollection<Error> errors { get; private set; }

		public List<string> savedFileNames { get; private set; }
		public List<string> archivedFileNames { get; private set; }

		public StorageFile currentSaveFile;
		public StorageFolder currentLoadFolder;
		public StorageFolder currentArchiveFolder;

		public bool autoLoad;

		public bool loadedThisLaunch;
		public bool running;

		public AppState() {
			this.files = new ObservableCollection<Flight>();
			this.errors = new ObservableCollection<Error>();

			this.savedFileNames = new List<string>();
			this.archivedFileNames = new List<string>();
		}

		internal void saveState() {
			ApplicationData.Current.RoamingSettings.Values["load"] = StorageApplicationPermissions.FutureAccessList.Add(this.currentLoadFolder);
			ApplicationData.Current.RoamingSettings.Values["save"] = StorageApplicationPermissions.FutureAccessList.Add(this.currentSaveFile);
			ApplicationData.Current.RoamingSettings.Values["archive"] = StorageApplicationPermissions.FutureAccessList.Add(this.currentArchiveFolder);
			ApplicationData.Current.RoamingSettings.Values["autoload"] = this.autoLoad;

			StringBuilder sFN = new StringBuilder();
			foreach (string s in this.savedFileNames) {
				sFN.Append(s + "\n");
			}
			ApplicationData.Current.RoamingSettings.Values[this.currentSaveFile.Path] = sFN.ToString();
		}

		internal async Task loadState() {
			bool needsLoadFolder = false;
			bool needsSaveFile = false;
			bool needsArchiveFolder = false;

			try {
				this.currentLoadFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync((string)ApplicationData.Current.RoamingSettings.Values["load"]);
			} catch (FileNotFoundException) {
				needsLoadFolder = true;
			} catch (ArgumentException) {
				needsLoadFolder = true;
			}

			try {
				this.currentSaveFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync((string)ApplicationData.Current.RoamingSettings.Values["save"]);
			} catch (FileNotFoundException) {
				needsSaveFile = true;
			} catch (ArgumentException) {
				needsSaveFile = true;
			}

			try {
				this.currentArchiveFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync((string)ApplicationData.Current.RoamingSettings.Values["archive"]);
			} catch (FileNotFoundException) {
				needsArchiveFolder = true;
			} catch (ArgumentException) {
				needsArchiveFolder = true;
			}

			this.autoLoad = (bool)ApplicationData.Current.RoamingSettings.Values["autoload"];

			if (needsArchiveFolder || needsLoadFolder || needsSaveFile) {
				var frame = new Frame();
				if (needsLoadFolder)
					frame.Navigate(typeof(Setup.ImportLocationPage), this);
				if (needsArchiveFolder)
					frame.Navigate(typeof(Setup.ArchiveLocationPage), this);
				if (needsSaveFile)
					frame.Navigate(typeof(Setup.ExportLocationPage), this);
				Window.Current.Content = frame;
			} else {
				this.savedFileNames = new List<String>(((string)ApplicationData.Current.RoamingSettings.Values[this.currentSaveFile.Path]).Split('\n'));

				if (this.autoLoad) {
					await this.refresh();
				}
			}

		}

		internal async Task refresh() {
			this.running = true;
			await this.loadFolder();
			await this.archiveFiles();
			await this.saveData();
			this.running = false;
			this.loadedThisLaunch = true;

			this.saveState();
		}

		internal async Task archiveFiles() {
			foreach (Flight f in this.files) {
				if (!this.archivedFileNames.Contains(f.file.Name)) {
					await f.file.CopyAsync(this.currentArchiveFolder, f.file.Name, NameCollisionOption.ReplaceExisting);
					this.archivedFileNames.Add(f.file.Name);
					this.filesArchived++;
					f.setArchived();
				}
			}
		}

		internal async Task loadFolder() {
			if (this.currentLoadFolder != null) {
				foreach (StorageFile file in await this.currentLoadFolder.GetFilesAsync()) {
					if (file.Name.EndsWith(".csv") && file.Name.StartsWith("log")) {
						await this.loadFile(file);
					}
				}
			} else
				this.errors.Add(new Error(null, "No load folder", "No load folder is selected."));
		}

		internal async Task loadFile(StorageFile file) {
			Flight f = null;
			try {
				f = await new CSVFile(file).getFileInfo();
			} catch (UnauthorizedAccessException) {
				this.errors.Add(new Error(file, file.Name, "Access to the file \"" + file.Name + "\" is denied. Is it open in another program?"));
			}
			if (f != null && !this.files.Contains(f)) {
				this.files.Add(f);
				f.setLoaded();
			}
		}

		internal async Task saveData() {
			if (this.currentSaveFile != null) {
				try {
					string fileString = await FileIO.ReadTextAsync(this.currentSaveFile);

					if (fileString.Length <= 150) {
						await FileIO.WriteTextAsync(this.currentSaveFile, Flight.COLUMN_HEADERS);
						this.savedFileNames = new List<string>();
					}

					foreach (Flight f in this.files) {
						if (!this.savedFileNames.Contains(f.file.Name)) {
							await FileIO.AppendTextAsync(this.currentSaveFile, f.summary + "\n");
							this.savedFileNames.Add(f.file.Name);
							this.filesSaved++;
							f.setSaved();
						}
					}
				} catch (FileNotFoundException) {
					ApplicationData.Current.RoamingSettings.Values[this.currentSaveFile.Path] = null;
				} catch (UnauthorizedAccessException) {
					this.errors.Add(new Error(this.currentSaveFile, this.currentSaveFile.Name, "Data could not be exported to \"" + this.currentSaveFile.Name + "\". Is it open in another program?"));
				}
			} else {
				var frame = new Frame();
				frame.Navigate(typeof(Setup.ImportLocationPage), this);
				Window.Current.Content = frame;
			}
		}

		internal static async Task<StorageFile> pickSaveFile() {
			FileSavePicker picker = new FileSavePicker();
			picker.SuggestedStartLocation = PickerLocationId.Desktop;
			picker.FileTypeChoices.Add("Comma Separated Value Spreadsheet", new List<string>() { ".csv" });
			picker.SuggestedFileName = "Flight Log";

			return await picker.PickSaveFileAsync();
		}

		internal static async Task<StorageFolder> pickLoadFolder() {
			FolderPicker picker = new FolderPicker();
			picker.ViewMode = PickerViewMode.Thumbnail;
			picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
			picker.FileTypeFilter.Add(".csv");

			return await picker.PickSingleFolderAsync();
		}

		internal static async Task<StorageFolder> pickArchiveFolder() {
			FolderPicker picker = new FolderPicker();
			picker.ViewMode = PickerViewMode.Thumbnail;
			picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
			picker.FileTypeFilter.Add(".csv");

			return await picker.PickSingleFolderAsync();
		}
	}
}
