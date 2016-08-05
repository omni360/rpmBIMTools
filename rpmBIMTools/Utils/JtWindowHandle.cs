using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace rpmBIMTools
{
    /// <summary>
    /// Wrapper class for converting 
    /// IntPtr to IWin32Window.
    /// </summary>
    public class JtWindowHandle : IWin32Window
    {
        IntPtr _hwnd;

        public JtWindowHandle(IntPtr h)
        {
            Debug.Assert(IntPtr.Zero != h,
              "expected non-null window handle");

            _hwnd = h;
        }

        public IntPtr Handle
        {
            get
            {
                return _hwnd;
            }
        }
    }

    public static class WindowsMessaging
    {
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
          int hWnd, int Msg, int wParam, int lParam);

        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
          int hWnd, int Msg, int wParam, int lParam);

        public const int WM_KEYDOWN = 0x0100;

        public static int SendWindowsMessage(
          int hWnd, int Msg, int wParam, int lParam)
        {
            int result = 0;
            if (hWnd > 0)
                result = SendMessage(hWnd, Msg, wParam, lParam);

            return result;
        }

        public static int PostWindowsMessage(
          int hWnd, int Msg, int wParam, int lParam)
        {
            int result = 0;
            if (hWnd > 0)
                result = PostMessage(hWnd, Msg, wParam, lParam);
            return result;
        }
    }

}