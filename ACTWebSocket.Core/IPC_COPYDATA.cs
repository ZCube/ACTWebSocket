using ACTWebSocket.Core;
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
using WebSocketSharp;

namespace ACTWebSocket_Plugin
{
    public delegate void OnOpen();
    public delegate void OnMessage(int code, string message);
    public interface IPC_Base
    {
        event OnMessage onMessage;
        event OnOpen onOpen;
        bool SendMessage(int code, string msg);
    }

    public partial class IPC_COPYDATA : Form, IPC_Base
    {
        public OnMessage onMessage;
        public OnOpen onOpen;
        String overlayCaption;


        public IPC_COPYDATA(String overlayCaption)
        {
            this.overlayCaption = overlayCaption;
            InitializeComponent();
        }

        event ACTWebSocket_Plugin.OnMessage IPC_Base.onMessage
        {
            add
            {
                this.onMessage += value;
            }

            remove
            {
                this.onMessage -= value;
            }
        }

        event OnOpen IPC_Base.onOpen
        {
            add
            {
            }

            remove
            {
            }
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

        public bool SendMessage(int code, string msg)
        {
            String caption = overlayCaption;
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

    public class IPC_WebSocket : IPC_Base
    {
        public OnMessage onMessage;
        public OnOpen onOpen;
        WebSocket ws = null;
        Boolean onOpend = false;
        int serverPort;
        long lastTimestamp;
        event ACTWebSocket_Plugin.OnMessage IPC_Base.onMessage
        {
            add
            {
                this.onMessage += value;
            }

            remove
            {
                this.onMessage -= value;
            }
        }

        event OnOpen IPC_Base.onOpen
        {
            add
            {
                this.onOpen += value;
            }

            remove
            {
                this.onOpen -= value;
            }
        }

        public IPC_WebSocket(int serverPort)
        {
            this.serverPort = serverPort;
            ConnectAsync(null);
        }

        void ConnectAsync(Action<WebSocket> callback)
        {
            lastTimestamp = Utility.ToUnixTimestamp(DateTime.UtcNow);
            if (ws != null)
            {
                ws.Close();
            }
            ws = new WebSocket("ws://localhost:" + serverPort.ToString());
            if (ws != null)
            {
                ws.OnMessage += (sender, e) =>
                {
                    onMessage(0, e.Data);
                };
                ws.OnOpen += (sender, e) =>
                {
                    if (callback != null)
                    {
                        this.onOpen();
                        callback(ws);
                    }
                };
                ws.ConnectAsync();
            }
        }

        public bool SendMessage(int code, string msg)
        {
            if (ws == null || (!ws.IsAlive && (Utility.ToUnixTimestamp(DateTime.UtcNow) - lastTimestamp) > 5))
            {
                ConnectAsync((ws) =>
                {
                    ws.SendAsync(msg, null);
                });
                return false;
            }
            ws.SendAsync(msg, null);
            Boolean b = ws.IsAlive;
            return ws.IsAlive;
        }
    }
}
