using System;

namespace Newtonsoft.Googleman.Common
{
    public static class EventExtensions
    {
        public static void Raise<TEventArgs>(this EventHandler<TEventArgs> del, object sender, TEventArgs args) where TEventArgs : EventArgs
        {
            if (del != null)
                del(sender, args);
        }

        public static void Raise(this EventHandler del, object sender)
        {
            if (del != null)
                del(sender, EventArgs.Empty);
        }
    }
}