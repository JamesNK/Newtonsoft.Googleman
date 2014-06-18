using System;
using System.Collections.Generic;
using System.Text;

namespace Newtonsoft.Googleman
{
    public interface ITray : IDisposable
    {
        bool RunAtStartup { get; set; }
        void Show();

        event EventHandler ChangeRunAtStartup;
        event EventHandler DisplaySplashScreen;
        event EventHandler ExitApplication;
        event EventHandler DisplayAboutDialog;
    }
}