using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Glass_Cockpit
{
    public class CSVFile
    {
        private List<Entry> data;
        private bool dataLoaded = false;

        public StorageFile file { get; private set; }

        public CSVFile(StorageFile file)
        {
            this.file = file;
            this.data = new List<Entry>();
        }

        public async Task<Flight> getFileInfo()
        {
            if (!dataLoaded) await this.loadData();
            return new Flight(this.file, this.getFlightTime(), this.getTakeoffTime(), this.getLandingTime(), this.getEngineTime(), this.getEngineStartTime(), this.getEngineStopTime(), this.getFromAirport(), this.getToAirport(), this.getDate());
        }

        private async Task loadData()
        {
            try
            {
                string tempData = null;
                var buffer = await FileIO.ReadBufferAsync(this.file);
                using (var dr = DataReader.FromBuffer(buffer))
                {
                    var bytes1251 = new Byte[buffer.Length];
                    dr.ReadBytes(bytes1251);

                    tempData = Encoding.GetEncoding("Windows-1251").GetString(bytes1251, 0, bytes1251.Length);
                }
                string[] lines = tempData.Replace("\\s", "").Replace(" ", "").Split('\n');

                for (int i = 3; i < lines.Length; i++)
                {
                    string[] temp = lines[i].Split(',');
                    if (temp.Length == Entry.ENTRY_LENGTH) this.data.Add(new Entry(temp));
                }
                this.dataLoaded = true;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        internal TimeSpan getFlightTime()
        {
            return this.getLandingTime() - this.getTakeoffTime();
            // return ((DateTime.Parse(this.getFlightInterval().stop.get(Entry.LclDate)) + TimeSpan.Parse(this.getFlightInterval().stop.get(Entry.LclTime))) - (DateTime.Parse(this.getFlightInterval().start.get(Entry.LclDate)) + TimeSpan.Parse(this.getFlightInterval().start.get(Entry.LclTime))));
        }

        private DateTime getTakeoffTime()
        {
            return this.getDate() + TimeSpan.Parse(this.getFlightInterval().start.get(Entry.LclTime)) + TimeSpan.FromSeconds(30);
        }

        private DateTime getLandingTime()
        {
            return this.getDate() + TimeSpan.Parse(this.getFlightInterval().stop.get(Entry.LclTime)) + TimeSpan.FromSeconds(30);
        }

        private EntryInterval getFlightInterval()
        {
            int takeoffIndex = 0;
            Entry takeoffEntry = null;
            double takeoffSpeed = -1;
            do
            {
                takeoffEntry = this.data.ElementAt(takeoffIndex);
                takeoffSpeed = takeoffEntry.getDouble(Entry.GndSpd);
                takeoffIndex++;
            }
            while (takeoffIndex < this.data.Count - 1 && takeoffSpeed < 50);

            int landingIndex = takeoffIndex;
            Entry landingEntry = null;
            double landingSpeed = -1;
            do
            {
                landingEntry = this.data.ElementAt(landingIndex);
                landingSpeed = landingEntry.getDouble(Entry.GndSpd);
                landingIndex++;
            }
            while (landingIndex < this.data.Count && landingSpeed > 50);
            landingIndex--;

            return new EntryInterval(takeoffEntry, landingEntry);
        }

        private TimeSpan getEngineTime()
        {
            return this.getEngineStopTime() - this.getEngineStartTime();
            // return ((DateTime.Parse(this.getEngineInterval().stop.get(Entry.LclDate)) + TimeSpan.Parse(this.getEngineInterval().stop.get(Entry.LclTime))) - (DateTime.Parse(this.getEngineInterval().start.get(Entry.LclDate)) + TimeSpan.Parse(this.getEngineInterval().start.get(Entry.LclTime))));
        }

        private DateTime getEngineStartTime()
        {
            return this.getDate() + TimeSpan.Parse(this.getEngineInterval().start.get(Entry.LclTime)) + TimeSpan.FromSeconds(30);
        }

        private DateTime getEngineStopTime()
        {
            return this.getDate() + TimeSpan.Parse(this.getEngineInterval().stop.get(Entry.LclTime)) + TimeSpan.FromSeconds(30);
        }

        private EntryInterval getEngineInterval()
        {
            int startIndex = 0;
            Entry startEntry = null;
            double startPressure = -1;
            do
            {
                startEntry = this.data.ElementAt(startIndex);
                startPressure = startEntry.getDouble(Entry.GndSpd);
                startIndex++;
            }
            while (startIndex < this.data.Count - 1 && startPressure < 50);

            int stopIndex = startIndex;
            Entry stopEntry = null;
            double stopPressure = -1;
            do
            {
                stopEntry = this.data.ElementAt(stopIndex);
                stopPressure = stopEntry.getDouble(Entry.GndSpd);
                stopIndex++;
            }
            while (stopIndex < this.data.Count && stopPressure > 50);
            stopIndex--;

            return new EntryInterval(startEntry, stopEntry);
        }

        private string getFromAirport()
        {
            return this.file.Name.Substring(this.file.Name.ToLower().LastIndexOf("_") + 1, this.file.Name.ToLower().IndexOf(".") - this.file.Name.ToLower().LastIndexOf("_") - 1).ToUpper();
        }

        private string getToAirport()
        {
            string latestKWpt = "";
            for (int i = this.data.Count - 1; latestKWpt.Length == 0 && i >= 0; i--)
            {
                string temp = this.data.ElementAt(i).get(Entry.AtvWpt);
                if (temp.Length > 0) latestKWpt = temp;
            }
            if (latestKWpt.Length == 0) return "None";
            else return latestKWpt;
        }

        private DateTime getDate()
        {
            try
            {
                return DateTime.Parse(this.getFlightInterval().start.get(Entry.LclDate));
            }
            catch (Exception)
            {
                return DateTime.Parse(this.getEngineInterval().start.get(Entry.LclDate));
            }
        }
    }
}
