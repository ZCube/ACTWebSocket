using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace ACTWebSocket.Core
{
    class Native
    {
        private static int ToIntPtr32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

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

        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;
        public const int WS_CHILD = 0x40000000;
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
                int result32 = GetWindowLong(hWnd, nIndex);
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
                int result32 = GetWindowLong(hWnd, nIndex);
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
                int result32 = SetWindowLong(hWnd, nIndex, ToIntPtr32(result));
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
                int result32 = GetWindowLong(hWnd, nIndex);
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
                int result32 = SetWindowLong(hWnd, nIndex, ToIntPtr32(result));
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

        public static List<string> SearchForWindow(string title)
        {
            SearchData sd = new SearchData { TitleKeyword = title };
            EnumWindows(new EnumWindowsProc(EnumProc), ref sd);
            return sd.overlayWindowTitles;
        }

        public static bool EnumProc(IntPtr hWnd, ref SearchData data)
        {
            // 종료 과정 중 자신의 프로세스 체크시 정지되는 경우 있음.
            // 만악의 근원
            uint id = 0;
            GetWindowThreadProcessId(hWnd, out id);
            if(GetCurrentProcessId() == id)
            {
                return true;
            }

            StringBuilder sb = new StringBuilder(1024);
            int ret = GetClassName(hWnd, sb, sb.Capacity);
            if(ret == 0)
            {
                return true;
            }
            sb = new StringBuilder(1024);
            ret = GetWindowText(hWnd, sb, sb.Capacity);
            if(ret != 0)
            {
                if (sb.ToString().StartsWith(data.TitleKeyword))
                {
                    data.overlayWindowTitles.Add(sb.ToString());
                }
            }
            return true;
        }

        public class SearchData
        {
            public string TitleKeyword = "";
            public List<string> overlayWindowTitles = new List<string>();
        }

        private delegate bool EnumWindowsProc(IntPtr hWnd, ref SearchData data);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        static extern uint GetCurrentProcessId();

        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, ref SearchData data);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        public const int WM_COPYDATA = 0x4A;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);

        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        public static void SendMessageToWindow(IntPtr hwnd, uint i, string str)
        {
            //byte[] buff = System.Text.Encoding.UTF8.GetBytes(str);
            string bs = Utility.Base64Encoding(str) + "\0";
            COPYDATASTRUCT cds = new COPYDATASTRUCT();
            cds.dwData = IntPtr.Zero;
            cds.cbData = bs.Length + 1;
            cds.lpData = bs;
            SendMessage(hwnd, WM_COPYDATA, new IntPtr(i), ref cds);
        }

        public static string Base64Encoding(string EncodingText, Encoding oEncoding = null)
        {
            if (oEncoding == null)
                oEncoding = Encoding.UTF8;

            byte[] arr = oEncoding.GetBytes(EncodingText);
            return Convert.ToBase64String(arr);
        }

        public static string Base64Decoding(string DecodingText, Encoding oEncoding = null)
        {
            if (oEncoding == null)
                oEncoding = Encoding.UTF8;

            byte[] arr = Convert.FromBase64String(DecodingText);
            return oEncoding.GetString(arr);
        }

    }
}
