using Prism.Events;

namespace Tongyu.Smart.Models
{
    public class Messenger : EventAggregator
    {
        private static Messenger _instance;

        public static Messenger Instance
        {
            get { return _instance ?? (_instance = new Messenger()); }
        }

        public static void ReleaseAll()
        {
            _instance = null;
        }
    }
}
