using Windows.Storage;

namespace Glass_Cockpit
{
    public class Error
    {
        public IStorageItem item { get; private set; }
        public string title { get; private set; }
        public string message { get; private set; }

        public Error(IStorageItem item, string title, string message)
        {
            this.item = item;
            this.title = title;
            this.message = message;
        }
    }
}
