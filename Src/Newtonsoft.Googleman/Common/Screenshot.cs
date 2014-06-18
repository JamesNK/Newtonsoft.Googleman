using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Newtonsoft.Googleman.Common
{
    public static class Screenshot
    {
        public static ImageSource GetScreenshot()
        {
            BitmapSource screencapture;

            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            using (Bitmap screenshot = new Bitmap(
                bounds.Width,
                bounds.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (Graphics graph = Graphics.FromImage(screenshot))
                {
                    graph.CopyFromScreen(
                        bounds.X,
                        bounds.Y,
                        0,
                        0,
                        bounds.Size,
                        CopyPixelOperation.SourceCopy);
                }

                IntPtr hBitmap = IntPtr.Zero;
                try
                {
                    hBitmap = screenshot.GetHbitmap();
                    screencapture = Imaging.CreateBitmapSourceFromHBitmap(
                        hBitmap,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromWidthAndHeight(bounds.Width, bounds.Height));
                }
                finally
                {
                    if (hBitmap != IntPtr.Zero)
                        DeleteObject(hBitmap);
                }
            }

            return screencapture;
        }

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}