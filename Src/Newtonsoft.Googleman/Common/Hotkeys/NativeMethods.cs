using System;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;

namespace Newtonsoft.Googleman.Common.Hotkeys
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct MARGINS
    {
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;

        public MARGINS(int left, int top, int right, int bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }

    [Serializable]
    public enum StdHandleType
    {
        Input = -10,
        Output = -11,
        Error = -12,
    }

    [Serializable]
    public enum ShowWindowCommand
    {
        Hide,
        Normal,
        ShowMinimized,
        ShowMaximized,
        ShowNoActivate,
        Show,
        Minimize,
        ShowMinNoActive,
        ShowNA,
        Restore,
        ShowDefault,
        ForceMinimize,
    }

    [Serializable]
    public enum GetWindowCommand
    {
        First,
        Last,
        Next,
        Previous,
        Owner,
        Child,
        Popup,
    }

    public static partial class NativeMethods
    {
        private static class Dll
        {
            public const string DwmApi = "dwmapi.dll";
            public const string Kernel32 = "kernel32";
            public const string User32 = "user32";
        }

        #region dwmapi!DwmExtendFrameIntoClientArea
        [DllImport(Dll.DwmApi, PreserveSig = false)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);
        #endregion

        #region dwmapi!DwmIsCompositionEnabled
        [DllImport(Dll.DwmApi, PreserveSig = false)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static extern bool DwmIsCompositionEnabled();
        #endregion

        #region kernel32!AllocConsole
        [return: MarshalAs(UnmanagedType.Bool)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [DllImport(Dll.Kernel32, SetLastError = true, ExactSpelling = true)]
        public static extern bool AllocConsole();
        #endregion

        #region kernel32!FreeConsole
        [DllImport(Dll.Kernel32, SetLastError = true, ExactSpelling = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeConsole();
        #endregion

        #region kernel32!GetConsoleWindow
        [DllImport(Dll.Kernel32, SetLastError = true, ExactSpelling = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static extern IntPtr GetConsoleWindow();
        #endregion

        #region kernel32!SetStdHandle
        [DllImport(Dll.Kernel32, SetLastError = true, ExactSpelling = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetStdHandle(StdHandleType nStdHandle, SafeHandle hHandle);
        #endregion

        #region user32!RegisterHotKey
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(Dll.User32, SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int uVirtKey);
        #endregion

        #region user32!SetForegroundWindow
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(Dll.User32, SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        #endregion

        #region user32!GetWindow
        [DllImport(Dll.User32, SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCommand command);
        #endregion

        #region user32!IsWindowVisible
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(Dll.User32, SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        #endregion

        #region user32!ShowWindow
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(Dll.User32, SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommand nCmdShow);
        #endregion

        #region user32!UnregisterHotKey
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport(Dll.User32, SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        #endregion
    }
}