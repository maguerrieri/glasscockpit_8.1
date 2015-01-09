
namespace Glass_Cockpit
{
    public class EntryInterval
    {
        private Entry first;
        private Entry last;

        public EntryInterval(Entry start, Entry stop)
        {
            this.first = start;
            this.last = stop;
        }

        public Entry start
        {
            get { return this.first; }
        }

        public Entry stop
        {
            get { return this.last; }
        }
    }
}
