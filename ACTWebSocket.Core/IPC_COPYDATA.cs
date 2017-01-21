using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACTWebSocket_Plugin
{
    public partial class IPC_COPYDATA : Form
    {
        public delegate void OnMessage(int code, string message) ;
        public OnMessage onMessage;
        public IPC_COPYDATA()
        {
            InitializeComponent();
        }

        private void Form_COPYDATA_Load(object sender, EventArgs e)
        {
            this.Hide();
        }

        [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, ref Win32API.COPYDATASTRUCT lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public class Win32API
        {
            public const Int32 WM_COPYDATA = 0x004A;
            public struct COPYDATASTRUCT
            {
                public IntPtr dwData;
                public int cbData;
                public IntPtr lpData;
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case Win32API.WM_COPYDATA:
                    Win32API.COPYDATASTRUCT cds = (Win32API.COPYDATASTRUCT)m.GetLParam(typeof(Win32API.COPYDATASTRUCT));
                    //Win32API.COPYDATASTRUCT cds2 = (Win32API.COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(Win32API.COPYDATASTRUCT));

                    byte[] data = new byte[cds.cbData];
                    Marshal.Copy(cds.lpData, data, 0, cds.cbData);

                    onMessage(cds.dwData.ToInt32(), Encoding.UTF8.GetString(data));
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);
        }

        public bool SendMessage(string caption, int code, string msg)
        {
            IntPtr hwnd = FindWindow(null, caption);
            if (hwnd != IntPtr.Zero)
            {
                Win32API.COPYDATASTRUCT cds = new Win32API.COPYDATASTRUCT();
                cds.dwData = new IntPtr(code);
                byte[] buff = Encoding.UTF8.GetBytes(msg);
                cds.lpData = Marshal.AllocHGlobal(buff.Length);
                Marshal.Copy(buff, 0, cds.lpData, buff.Length);
                cds.cbData = buff.Length;
                SendMessage(hwnd, Win32API.WM_COPYDATA, this.Handle, ref cds);
                return true;
            }
            return false;
        }

    }
}
