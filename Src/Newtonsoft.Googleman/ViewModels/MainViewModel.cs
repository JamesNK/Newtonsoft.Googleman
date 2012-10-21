using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using Newtonsoft.Googleman.Common;
using Newtonsoft.Googleman.Views;

namespace Newtonsoft.Googleman.ViewModels
{
  public class MainViewModel : ViewModelBase
  {
    private readonly object _lock = new object();
    private bool _displaySplashScreen;

    public event EventHandler SplashScreenComplete;

    private MainView MainView
    {
      get { return (MainView) View; }
    }

    protected internal override void Loaded()
    {
      Storyboard sb = (Storyboard)View.TryFindResource("sb");
      sb.Completed += SplashScreenAnimationComplete;
    }

    private void SetupSplashScreen(ImageSource backgroundImage)
    {
      Logger.Debug("Setting up splash screen");

      Screen s = Screen.PrimaryScreen;

      MainView.WindowStartupLocation = WindowStartupLocation.Manual;
      MainView.Left = s.Bounds.Left;
      MainView.Top = s.Bounds.Top;
      MainView.Width = s.Bounds.Width;
      MainView.Height = s.Bounds.Height;
      MainView.Topmost = true;
      MainView.ShowInTaskbar = false;
      MainView.WindowState = WindowState.Maximized;

      SetupLogoImage();
      SetupBackgroundImage(backgroundImage);

      MainView.ParentGroup.ClipGeometry = new RectangleGeometry(new Rect(new Size(s.Bounds.Width, s.Bounds.Height)));
    }

    private void SetupBackgroundImage(ImageSource backgroundImage)
    {
      MainView.BackgroundRectangle.ImageSource = backgroundImage;
      MainView.BackgroundRectangle.Rect = new Rect(new Size(MainView.Width, MainView.Height));

      MainView.BackgroundRotate.CenterX = MainView.Width/2;
      MainView.BackgroundRotate.CenterY = MainView.Height/2;

      MainView.LogoScale.CenterX = MainView.Width/2;
      MainView.LogoScale.CenterY = MainView.Height/2;
    }

    private void SetupLogoImage()
    {
      BitmapImage logoImage = new BitmapImage();

      logoImage.BeginInit();
      logoImage.UriSource = new Uri("pack://application:,,,/Resources/google_logo.jpg", UriKind.Absolute);
      logoImage.EndInit();

      MainView.LogoRectangle.ImageSource = logoImage;

      Size unscaledLogoSize = new Size(36, 15);

      double scale = MainView.Width/unscaledLogoSize.Width;
      Size scaledLogoSize = new Size(36*scale, 15*scale);

      double left = (MainView.Width/2) - (scaledLogoSize.Width/2);
      double top = (MainView.Height/2) - (scaledLogoSize.Height/2);

      MainView.LogoRectangle.Rect = new Rect(new Point(left, top), scaledLogoSize);

      MainView.LogoScale.ScaleX = unscaledLogoSize.Width / MainView.Width;
      MainView.LogoScale.ScaleY = unscaledLogoSize.Width / MainView.Width;
    }

    public void DisplaySplashScreen(ImageSource backgroundImage)
    {
      if (!_displaySplashScreen)
      {
        lock (_lock)
        {
          if (!_displaySplashScreen)
          {
            SetupSplashScreen(backgroundImage);

            Logger.Debug("Showing splash screen");
            Show();

            StartAnimation();

            Thread.MemoryBarrier();
            _displaySplashScreen = true;
          }
        }
      }
    }

    private void StartAnimation()
    {
      Storyboard sb = (Storyboard)View.TryFindResource("sb");

      Logger.Debug("Start splash screen animation");
      sb.Begin();

      StreamResourceInfo info = System.Windows.Application.GetResourceStream(new Uri("/Resources/batman.wav", UriKind.Relative));
      using (Stream wavStream = info.Stream)
      using (SoundPlayer soundPlayer = new SoundPlayer(wavStream))
      {
        Logger.Debug("Start splash screen sound");
        soundPlayer.Play();
      }
    }

    private void SplashScreenAnimationComplete(object sender, EventArgs e)
    {
      MainView mainView = (MainView)View;

      mainView.BackgroundRectangle.ImageSource = null;
      mainView.LogoRectangle.ImageSource = null;

      Hide();

      SplashScreenComplete.Raise(this);

      _displaySplashScreen = false;
    }
  }
}