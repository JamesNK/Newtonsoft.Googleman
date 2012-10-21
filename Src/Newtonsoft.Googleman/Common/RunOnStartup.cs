using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Newtonsoft.Googleman.Common
{
  public class RunOnStartup
  {
    /// <summary>
    /// Runs the Program on Startup.
    /// </summary>
    /// <param name="runOnStartup">True to Run on Startup, False to NOT Run on Startup.</param>
    public void SetRunStartup(Boolean runOnStartup)
    {
      RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

      if (runOnStartup)
        key.SetValue(Constants.ApplicationName, Application.ExecutablePath);
      else
        key.DeleteValue(Constants.ApplicationName, false);
    }

    /// <summary>
    /// Gets or Sets if the Program will Run on Startup.
    /// </summary>
    public bool GetRunStartup()
    {
      RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");

      return (key != null && key.GetValue(Constants.ApplicationName) != null);
    }
  }
}