using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading;
using System.Windows;
using Newtonsoft.Googleman.Common;
using Newtonsoft.Googleman.ViewModels;
using Newtonsoft.Googleman.Views;

namespace Newtonsoft.Googleman
{
    public partial class App : Application
    {
        private static readonly Mutex ApplicationMutex = new Mutex(true, "{2BD07755-B672-4cad-9F71-6C3244D6470A}");

        private GooglemanApplication _application;

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            Logger.Setup();

            // check whether application is already running
            if (ApplicationMutex.WaitOne(TimeSpan.Zero, true))
            {
                WindowManager.Instance = new WindowManager(new Dictionary<Type, Type>
                {
                    { typeof(AboutViewModel), typeof(AboutView) },
                    { typeof(MainViewModel), typeof(MainView) },
                });

                _application = new GooglemanApplication();
                _application.Initialize();

                ApplicationMutex.ReleaseMutex();
            }
            else
            {
                // already running, quit new instance
                Shutdown();
            }
        }

        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            if (_application != null)
                _application.Dispose();
        }
    }
}