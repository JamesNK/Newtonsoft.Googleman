using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using Newtonsoft.Googleman.Common;

namespace Newtonsoft.Googleman.ViewModels
{
  public class AboutViewModel : ViewModelBase
  {
    public ICommand OkCommand { get; private set; }
    public ICommand LaunchHomepageCommand { get; private set; }

    public string Version
    {
      get
      {
        Version version = Assembly.GetExecutingAssembly().GetName().Version;
        return Constants.ApplicationName + ", Version " + version.Major + "." + version.Minor;
      }
    }

    public AboutViewModel()
    {
      OkCommand = new DelegateCommand(OnOk);
      LaunchHomepageCommand = new DelegateCommand(OnLaunchHomepage);
    }

    private void OnOk()
    {
      Close();
    }

    private void OnLaunchHomepage()
    {
      Process.Start("http://james.newtonking.com");
    }
  }
}