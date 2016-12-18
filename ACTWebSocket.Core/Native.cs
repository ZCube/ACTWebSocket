using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ACTWebSocket.Core
{
    class Native
    {
        private static int ToIntPtr32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool CloseWindow(IntPtr hWnd);

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
        public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int WS_EX_NOACTIVATE = 0x08000000;

        public static IntPtr GetWindowLongFlag(IntPtr hWnd, IntPtr value)
        {
            int nIndex = GWL_EXSTYLE;
            int error = 0;
            IntPtr result = IntPtr.Zero;

            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                Int32 result32 = GetWindowLong(hWnd, nIndex);
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(result32);
            }
            else
            {
                result = GetWindowLongPtr(hWnd, nIndex);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            result = new IntPtr(result.ToInt64() & value.ToInt64());

            return result;
        }

        public static IntPtr SetWindowLongFlag(IntPtr hWnd, IntPtr value)
        {
            int nIndex = GWL_EXSTYLE;
            int error = 0;
            IntPtr result = IntPtr.Zero;

            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                Int32 result32 = GetWindowLong(hWnd, nIndex);
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(result32);
            }
            else
            {
                result = GetWindowLongPtr(hWnd, nIndex);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            result = new IntPtr(result.ToInt64() | value.ToInt64());

            if (IntPtr.Size == 4)
            {
                Int32 result32 = SetWindowLong(hWnd, nIndex, ToIntPtr32(result));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(result32);
            }
            else
            {
                result = SetWindowLongPtr(hWnd, nIndex, result);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        public static IntPtr UnsetWindowLongFlag(IntPtr hWnd, IntPtr value)
        {
            int nIndex = GWL_EXSTYLE;
            int error = 0;
            IntPtr result = IntPtr.Zero;

            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                Int32 result32 = GetWindowLong(hWnd, nIndex);
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(result32);
            }
            else
            {
                result = GetWindowLongPtr(hWnd, nIndex);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            result = new IntPtr(result.ToInt64() & ~value.ToInt64());

            if (IntPtr.Size == 4)
            {
                Int32 result32 = SetWindowLong(hWnd, nIndex, ToIntPtr32(result));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(result32);
            }
            else
            {
                result = SetWindowLongPtr(hWnd, nIndex, result);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        public static List<String> SearchForWindow(string title)
        {
            SearchData sd = new SearchData { TitleKeyword = title };
            EnumWindows(new EnumWindowsProc(EnumProc), ref sd);
            return sd.overlayWindowTitles;
        }

        public static bool EnumProc(IntPtr hWnd, ref SearchData data)
        {
            // Check classname and title 
            // This is different from FindWindow() in that the code below allows partial matches
            StringBuilder sb = new StringBuilder(1024);
            GetWindowText(hWnd, sb, sb.Capacity);
            if (sb.ToString().StartsWith(data.TitleKeyword))
            {
                data.overlayWindowTitles.Add(sb.ToString());
            }
            return true;
        }

        public class SearchData
        {
            public String TitleKeyword = "";
            public List<String> overlayWindowTitles = new List<String>();
        }

        private delegate bool EnumWindowsProc(IntPtr hWnd, ref SearchData data);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, ref SearchData data);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        const int WM_COPYDATA = 0x4A;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.SafeArray)]
            public byte[] lpData;
        }

        public static void SendMessageToWindow(IntPtr hwnd, uint i, string str)
        {
            byte[] buff = System.Text.Encoding.UTF8.GetBytes(str);
            COPYDATASTRUCT cds = new COPYDATASTRUCT();
            cds.dwData = IntPtr.Zero;
            cds.cbData = buff.Length + 1;
            cds.lpData = buff;

            SendMessage(hwnd, WM_COPYDATA, new IntPtr(i), ref cds);
        }
    }
}
