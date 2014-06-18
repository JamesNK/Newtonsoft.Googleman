using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Resources;
using Newtonsoft.Googleman.Common;
using Application = System.Windows.Application;

namespace Newtonsoft.Googleman
{
    public class Tray : ITray
    {
        private NotifyIcon _notifyIcon;
        private ToolStripMenuItem _runAtStartupMenuItem;

        public Tray()
        {
            CreateNotifyIcon();
        }

        private void CreateNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
            StreamResourceInfo info = Application.GetResourceStream(new Uri("/Resources/google.ico", UriKind.Relative));
            using (Stream iconStream = info.Stream)
            {
                _notifyIcon.Icon = new Icon(iconStream);
            }
            _notifyIcon.Visible = false;
            _notifyIcon.DoubleClick += (sender, args) => DisplaySplashScreen.Raise(this);

            ToolStripMenuItem toTheGoogleCaveMenuItem = new ToolStripMenuItem { Text = "To the GoogleCave!" };
            toTheGoogleCaveMenuItem.Font = new Font(toTheGoogleCaveMenuItem.Font, FontStyle.Bold);
            toTheGoogleCaveMenuItem.Click += (sender, args) => DisplaySplashScreen.Raise(this);
            toTheGoogleCaveMenuItem.ShortcutKeyDisplayString = "Win+G";

            _runAtStartupMenuItem = new ToolStripMenuItem();
            _runAtStartupMenuItem.Text = "Start with Windows";
            _runAtStartupMenuItem.Click += (sender, args) => ChangeRunAtStartup.Raise(this);

            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.AddRange(
                new ToolStripItem[]
                {
                    toTheGoogleCaveMenuItem,
                    _runAtStartupMenuItem,
                    new ToolStripMenuItem("About", null, (sender, args) => DisplayAboutDialog.Raise(this)),
                    new ToolStripSeparator(),
                    new ToolStripMenuItem("Exit", null, (sender, args) => ExitApplication.Raise(this))
                });
        }

        public void Show()
        {
            _notifyIcon.Visible = true;
        }

        public bool RunAtStartup
        {
            get { return _runAtStartupMenuItem.Checked; }
            set { _runAtStartupMenuItem.Checked = value; }
        }

        public event EventHandler ChangeRunAtStartup;
        public event EventHandler DisplaySplashScreen;
        public event EventHandler DisplayAboutDialog;
        public event EventHandler ExitApplication;

        public void Dispose()
        {
            _notifyIcon.Dispose();
        }
    }
}