using System;
using System.Windows;
using System.Windows.Interop;

namespace Newtonsoft.Googleman.Common.Hotkeys
{
  public static class WindowSwitcher
  {
    public static void ActivateNextWindow()
    {
      ActivateNextWindow(Application.Current.MainWindow);
    }

    public static void ActivateNextWindow(Window current)
    {
      IntPtr next = GetNextWindow(current);

      NativeMethods.ShowWindow(next, ShowWindowCommand.Show);
      NativeMethods.SetForegroundWindow(next);
    }

    private static IntPtr GetNextWindow(Window relativeTo)
    {
      IntPtr current = GetWindowHandle(relativeTo);
      IntPtr next = NativeMethods.GetWindow(current, GetWindowCommand.Next);

      while (!NativeMethods.IsWindowVisible(next))
      {
        next = NativeMethods.GetWindow(next, GetWindowCommand.Next);
      }

      return next;
    }

    private static IntPtr GetWindowHandle(Window window)
    {
      return new WindowInteropHelper(window).Handle;
    }
  }
}