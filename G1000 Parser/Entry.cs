using System;

namespace Glass_Cockpit
{
    public class Entry
    {
        public static readonly int LclDate = 0;
        public static readonly int LclTime = 1;
        public static readonly int UTCOfst = 2;
        public static readonly int AtvWpt = 3;
        public static readonly int Latitude = 4;
        public static readonly int Longitude = 5;
        public static readonly int AltB = 6;
        public static readonly int BaroA = 7;
        public static readonly int AltMSL = 8;
        public static readonly int OAT = 9;
        public static readonly int IAS = 10;
        public static readonly int GndSpd = 11;
        public static readonly int VSpd = 12;
        public static readonly int Pitch = 13;
        public static readonly int Roll = 14;
        public static readonly int LatAc = 15;
        public static readonly int NormAc = 16;
        public static readonly int HDG = 17;
        public static readonly int TRK = 18;
        public static readonly int volt1 = 19;
        public static readonly int amp1 = 20;
        public static readonly int FQtyL = 21;
        public static readonly int FQtyR = 22;
        public static readonly int E1FFlow = 23;
        public static readonly int E1OilT = 24;
        public static readonly int E1OilP = 25;
        public static readonly int E1MAP = 26;
        public static readonly int E1RPM = 27;
        public static readonly int E1CHT1 = 28;
        public static readonly int E1CHT2 = 29;
        public static readonly int E1CHT3 = 30;
        public static readonly int E1CHT4 = 31;
        public static readonly int E1CHT5 = 32;
        public static readonly int E1CHT6 = 33;
        public static readonly int E1TIT1 = 34;
        public static readonly int AltGPS = 35;
        public static readonly int TAS = 36;
        public static readonly int HSIS = 37;
        public static readonly int CRS = 38;
        public static readonly int NAV1 = 39;
        public static readonly int NAV2 = 40;
        public static readonly int COM1 = 41;
        public static readonly int COM2 = 42;
        public static readonly int HCDI = 43;
        public static readonly int VCDI = 44;
        public static readonly int WndSpd = 45;
        public static readonly int WndDr = 46;
        public static readonly int WptDst = 47;
        public static readonly int WptBrg = 48;
        public static readonly int MagVar = 49;
        public static readonly int AfcsOn = 50;
        public static readonly int RollM = 51;
        public static readonly int PitchM = 52;
        public static readonly int RollC = 53;
        public static readonly int PichC = 54;
        public static readonly int VSpdG = 55;
        public static readonly int GPSfix = 56;
        public static readonly int HAL = 57;
        public static readonly int VAL = 58;
        public static readonly int HPLwas = 59;
        public static readonly int HPLfd = 60;
        public static readonly int VPLwas = 61;

        public static readonly int ENTRY_LENGTH = 62;

        private String[] data;

        public Entry(String[] parsedData)
        {
            //this.data = parsedData;
            this.data = new string[Entry.ENTRY_LENGTH];
            this.data[Entry.E1OilP] = parsedData[Entry.E1OilP];
            this.data[Entry.GndSpd] = parsedData[Entry.GndSpd];
            this.data[Entry.AtvWpt] = parsedData[Entry.AtvWpt];
            this.data[Entry.LclDate] = parsedData[Entry.LclDate];
            this.data[Entry.LclTime] = parsedData[Entry.LclTime];
        }

        public Entry(String rawData)
        {
            string[] tempData = rawData.Split(',');
            this.data = new string[Entry.ENTRY_LENGTH];
            this.data[Entry.E1OilP] = tempData[Entry.E1OilP];
            this.data[Entry.GndSpd] = tempData[Entry.GndSpd];
            this.data[Entry.AtvWpt] = tempData[Entry.AtvWpt];
            this.data[Entry.LclDate] = tempData[Entry.LclDate];
            this.data[Entry.LclTime] = tempData[Entry.LclTime];
        }

        private Boolean isNumValue(int index)
        {
            return index == E1OilP || index == GndSpd;
        }

        public double getDouble(int index)
        {
            String temp = data[index];
            if (isNumValue(index) && temp.Length != 0)
            {
                return Double.Parse(temp);
            }
            else return -1;
        }

        public String get(int index)
        {
            return data[index];
        }
    }
}