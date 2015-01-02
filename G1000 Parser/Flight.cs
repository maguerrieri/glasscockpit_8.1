using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Windows.Storage;

namespace Glass_Cockpit {
	public class Flight : IComparable, INotifyPropertyChanged {
		public static string COLUMN_HEADERS { // = "Date,Takeoff Airport,Landing Airport,Engine Start Time,Engine Stop Time,Engine Duration,Takeoff Time,Landing Time,Flight Duration,Data File Name\n";
			get {
				StringBuilder s = new StringBuilder();
				foreach (string head in COLUMNS.Keys) {
					s.Append(head + ",");
				}
				s.Remove(s.Length - 1, 1);
				s.Append("\n");
				return s.ToString();
			}
		}
		public delegate string column(Flight file);
		public static Dictionary<string, column> COLUMNS {
			get {
				return new Dictionary<string, column> {
                    { "Date", (file) => file.dateString }, 
                    { "Takeoff Airport", (file) => file.fromAirport }, 
                    { "Landing Airport", (file) => file.toAirport }, 
                    { "Engine Start Time", (file) => file.engineStartTimeString }, 
                    { "Takeoff Time", (file) => file.takeoffTimeString }, 
                    { "Landing Time", (file) => file.landingTimeString }, 
                    { "Engine Stop Time", (file) => file.engineStopTimeString }, 
                    { "Flight Duration", (file) => file.flightTimeString }, 
                    { "Engine Duration", (file) => file.engineTimeString }, 
                    { "Data File Name", (file) => file.title }
                };
			}
		}

		public string title { get { return this.file.Name; } }
		public StorageFile file { get; private set; }

		public string status {
			get {
				if (this.loaded) {
					if (this.archived) {
						if (this.saved) return "Loaded, Archived, Saved.";
						else return "Loaded, Archived.";
					} else return "Loaded.";
				} else return "Not loaded.";
			}
		}
		private bool loaded;
		private bool saved;
		private bool archived;

		public TimeSpan flightTime { get; set; }
		public string flightTimeString { get { return this.flightTime.ToString(@"hh\:mm"); } }

		public DateTime takeoffTime { get; set; }
		public string takeoffTimeString { get { return this.takeoffTime.ToString("t"); } }

		public DateTime landingTime { get; set; }
		public string landingTimeString { get { return this.landingTime.ToString("t"); } }

		public TimeSpan engineTime { get; set; }
		public string engineTimeString { get { return this.engineTime.ToString(@"hh\:mm"); } }

		public DateTime engineStartTime { get; set; }
		public string engineStartTimeString { get { return this.engineStartTime.ToString("t"); } }

		public DateTime engineStopTime { get; set; }
		public string engineStopTimeString { get { return this.engineStopTime.ToString("t"); } }

		public string fromAirport { get; private set; }
		public string toAirport { get; private set; }

		public DateTime date { get; set; }
		public string dateString { get { return this.date.ToString("d"); } }

		public string summary {
			get {
				return this.dateString + "," + this.fromAirport + "," +
					this.toAirport + "," + this.engineStartTimeString + "," +
					this.engineStopTimeString + "," + this.engineTimeString + "," +
					this.takeoffTimeString + "," + this.landingTimeString + "," +
					this.flightTimeString + "," + this.title;
			}
		}

		internal Flight(StorageFile file,
						TimeSpan flightTime,
						DateTime takeoffTime,
						DateTime landingTime,
						TimeSpan engineTime,
						DateTime engineStartTime,
						DateTime engineStopTime,
						string fromAirport,
						string toAirport,
						DateTime date
							) {
			this.file = file;
			this.flightTime = flightTime;
			this.takeoffTime = takeoffTime;
			this.landingTime = landingTime;
			this.engineTime = engineTime;
			this.engineStartTime = engineStartTime;
			this.engineStopTime = engineStopTime;
			this.fromAirport = fromAirport;
			this.toAirport = toAirport;
			this.date = date;
		}

		int IComparable.CompareTo(object obj) {
			Flight newObj = obj as Flight;

			if (newObj != null)
				return -this.date.Ticks.CompareTo(newObj.date.Ticks);
			else
				throw new Exception("Can't compare a CSVFileInfo to a different type");
		}

		public override bool Equals(object obj) {
			Flight newObj = obj as Flight;

			return newObj != null && this.file.Equals(newObj.file);
		}

		public override int GetHashCode() {
			return this.file.Name.GetHashCode();
		}

		public void setLoaded() {
			this.loaded = true;
			this.NotifyPropertyChanged();
		}

		public void setSaved() {
			this.saved = true;
			this.NotifyPropertyChanged();
		}

		public void setArchived() {
			this.archived = true;
			this.NotifyPropertyChanged();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged() {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs("status"));
			}
		}
	}
}
