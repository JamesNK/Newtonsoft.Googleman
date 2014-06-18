using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Newtonsoft.Googleman.Common
{
    public static class Logger
    {
        [Conditional("DEBUG")]
        public static void Setup()
        {
            Trace.AutoFlush = true;
            Trace.Listeners.Add(new TextWriterTraceListener(DateTime.Now.ToString("HHmmss") + "-logger.log"));
        }

        [Conditional("DEBUG")]
        public static void Debug(string message)
        {
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("o") + " - " + message);
        }
    }
}