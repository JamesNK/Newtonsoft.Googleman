using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Googleman.Common;
using Newtonsoft.Googleman.Common.Hotkeys;
using Newtonsoft.Googleman.ViewModels;
using Newtonsoft.Googleman.Views;

namespace Newtonsoft.Googleman
{
  public class GooglemanApplication : IDisposable
  {
    private RunOnStartup _runOnStartup;
    private HotkeyManager _hotkeyManager;
    private ITray _tray;
    private MainViewModel _mainViewModel;
    
    public void Initialize()
    {
      _runOnStartup = new RunOnStartup();

      _mainViewModel = new MainViewModel();
      _mainViewModel.SplashScreenComplete += SplashScreenComplete;
      WindowManager.Instance.ShowWindow(_mainViewModel);

      _hotkeyManager = new HotkeyManager(_mainViewModel.View);
      try
      {
        _hotkeyManager.Register(new Hotkey(ModifierKeys.Windows, Key.G));
      }
      catch (Win32Exception)
      {
        // hotkey already registered by another program
      }
      _hotkeyManager.HotkeyPressed += (sender, args) => DisplaySplashScreen();

      _tray = new Tray();
      _tray.RunAtStartup = _runOnStartup.GetRunStartup();

      _tray.ChangeRunAtStartup += ChangeRunAtStartup;
      _tray.ExitApplication += ExitApplication;
      _tray.DisplayAboutDialog += DisplayAboutDialog;
      _tray.DisplaySplashScreen += (sender, args) => DisplaySplashScreen();
      _tray.Show();
    }

    private void DisplaySplashScreen()
    {
      Logger.Debug("Getting screenshot");
      var screenshot = Screenshot.GetScreenshot();

      _mainViewModel.DisplaySplashScreen(screenshot);

      Logger.Debug("Finished starting splash screen");
    }

    private void SplashScreenComplete(object sender, EventArgs eventArgs)
    {
      Logger.Debug("Splash screen complete");

      // launch Google in a background thread to avoid hanging the UI
      BackgroundWorker worker = new BackgroundWorker();
      worker.DoWork += (o, args) => Process.Start(@"http://www.google.com");
      worker.RunWorkerAsync();
    }

    private void DisplayAboutDialog(object sender, EventArgs e)
    {
      var aboutDialog = new AboutViewModel();

      WindowManager.Instance.ShowDialog(aboutDialog);
    }

    private void ExitApplication(object sender, EventArgs e)
    {
      Application.Current.Shutdown();
    }

    void ChangeRunAtStartup(object sender, EventArgs e)
    {
      bool current = _tray.RunAtStartup;
      try
      {
        _tray.RunAtStartup = !current;
        _runOnStartup.SetRunStartup(_tray.RunAtStartup);
      }
      catch
      {
        _tray.RunAtStartup = current;
        MessageBox.Show("You do not have permission to change this.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    public void Dispose()
    {
      if (_tray != null)
        _tray.Dispose();
    }
  }
}