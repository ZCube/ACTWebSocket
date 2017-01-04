using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using System.Xml;

namespace ACTWebSocket_Plugin
{
    using static ACTWebSocketCore;
    using ACTWebSocket.Core;
    using ACTWebSocket.Core.Classes.Interfaces;
    using CefSharp;
    using CefSharp.WinForms;
    using Classes;
    using Newtonsoft.Json.Linq;
    using Open.Nat;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using System.Threading;
    using System.Net;

    public interface PluginDirectory
    {
        void SetPluginDirectory(string path);
        string GetPluginDirectory();
    }
    public class ACTWebSocketMain : UserControl, IActPluginV1, PluginDirectory
    {

        string overlaySkinDirectory { get; set; }
        string pluginDirectory = "";
        private ChromiumWebBrowser browser;

        public void SetSkinDir(string path)
        {
            overlaySkinDirectory = path;
        }

        public string GetSkinDir()
        {
            return overlaySkinDirectory;
        }

        public void SetPluginDirectory(string path)
        {
            pluginDirectory = path;
        }

        public string GetPluginDirectory()
        {
            return pluginDirectory;
        }
        private ACTWebSocketCore core;
        #region Designer Created Code (Avoid editing)
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ACTWebSocketMain));
            this.SuspendLayout();
            // 
            // ACTWebSocketMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Name = "ACTWebSocketMain";
            this.Load += new System.EventHandler(this.ACTWebSocket_Load);
            this.ResumeLayout(false);

        }

        #endregion

        #endregion
        public ACTWebSocketMain()
        {
            InitializeComponent();
            InitBrowser();
        }

        public bool ChatFilter { get; set; }

        //CEFWorks CEF;
        public void InitBrowser()
        {
            ChatFilter = false;
            CefLibraryHandle libraryLoader = new CefLibraryHandle(Settings.CEFLIB);
            CefSettings settings = new CefSettings();


            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = CefSharpSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new CefSharpSchemeHandlerFactory()
            });
            settings.BrowserSubprocessPath = Settings.CEFBRW;
            settings.LocalesDirPath = Settings.CEFLOC;
            settings.UserDataPath = Settings.CEFUSR;
            settings.CachePath = Settings.CEFBIN;
            settings.LogFile = Settings.LOGDIR + "\\debug.log";

            // 3264
            if (Environment.Is64BitOperatingSystem)
                settings.ResourcesDirPath = Settings.CEFDIR;
            else
                settings.ResourcesDirPath = Settings.CEFDIR64;

            CultureInfo ci = CultureInfo.InstalledUICulture;
            string specName = "en";
            try { specName = CultureInfo.CreateSpecificCulture(ci.Name).Name; } catch { }
            settings.Locale = specName;

            Cef.Initialize(settings);

            browser = new ChromiumWebBrowser("rsrc://localhost/Resources/MainForm.html");
            browser.FrameLoadEnd += Browser_FrameLoadEnd;

            Controls.Add(browser);
            browser.RegisterJsObject("main", this);

            browser.Dock = DockStyle.Fill;
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            String strHostName = string.Empty;
            strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);

            List<String> addrs = new List<String>();
            addrs.Add("127.0.0.1");
            {
                IPAddress[] addr = ipEntry.AddressList;
                for (int i = 0; i < addr.Length; i++)
                {
                    if (addr[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        addrs.Add(addr[i].ToString());
                }
            }
            String ipaddress = Utility.GetExternalIp();
            if (ipaddress.Length > 0)
                addrs.Add(ipaddress);

            addrs = Utility.Distinct<String>(addrs);
            addrs.Sort();

            StringBuilder sb = new StringBuilder();
            foreach (var addr in addrs)
            {
                sb.Append("addHostname(\"" + addr + "\");");
            }
            sb.Append("updateWebsocketSettings();");
            browser.ExecuteScriptAsync(sb.ToString());
        }

        ~ACTWebSocketMain()
        {
            SaveSettings();
        }

        Label lblStatus;    // The status label that appears in ACT's Plugin tab
        string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\ACTWebSocket.config.json");
        string overlaySettingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\ACTWebSocket.overlay.json");

        #region IActPluginV1 Members

        static List<KeyValuePair<string, MiniParseSortType>> sortTypeDict = new List<KeyValuePair<string, MiniParseSortType>>()
        {
            new KeyValuePair<string, MiniParseSortType>("DoNotSort", MiniParseSortType.None),
            new KeyValuePair<string, MiniParseSortType>("SortStringAscending", MiniParseSortType.StringAscending),
            new KeyValuePair<string, MiniParseSortType>("SortStringDescending", MiniParseSortType.StringDescending),
            new KeyValuePair<string, MiniParseSortType>("SortNumberAscending", MiniParseSortType.NumericAscending),
            new KeyValuePair<string, MiniParseSortType>("SortNumberDescending", MiniParseSortType.NumericDescending)
        };

        JObject overlayWindows = new JObject(); // 설정 전부

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            if (core == null)
            {
                core = new ACTWebSocketCore();
                core.pluginDirectory = pluginDirectory;
                core.overlaySkinDirectory = overlaySkinDirectory;
                core.hwnd = Handle;
            }
            lblStatus = pluginStatusText;   // Hand the status label's reference to our local var
            pluginScreenSpace.Controls.Add(this);   // Add this UserControl to the tab ACT provides
            Dock = DockStyle.Fill; // Expand the UserControl to fill the tab's client space
            LoadSettings();



            //if (core != null)
            //{
            //    core.Filters["/BeforeLogLineRead"] = BeforeLogLineReadUse.Checked;
            //    core.Filters["/OnLogLineRead"] = OnLogLineReadUse.Checked;
            //    core.Filters["/MiniParse"] = MiniParseUse.Checked;
            //    core.Config.SortKey = MiniParseSortKey.Text.Trim();
            //    core.Config.SortType = (MiniParseSortType)sortType.SelectedIndex;
            //}
            //try
            //{
            //    core.StartUIServer();

            //    if (autostart.Checked)
            //    {
            //        StartServer();
            //    }
            //    else
            //    {
            //        StopServer();
            //    }
            //}
            //catch(Exception e)
            //{
            //    MessageBox.Show(e.Message);
            //    core.StopUIServer();
            //    StopServer();
            //}
            // Create some sort of parsing event handler.  After the "+=" hit TAB twice and the code will be generated for you.
            ActGlobals.oFormActMain.BeforeLogLineRead += oFormActMain_BeforeLogLineRead;
            ActGlobals.oFormActMain.OnLogLineRead += oFormActMain_OnLogLineRead;
            var s = ActGlobals.oFormActMain.ActPlugins;
            lblStatus.Text = "Plugin Started";
        }

        public void DeInitPlugin()
        {
            //StopServer();
            // Unsubscribe from any events you listen to when exiting!
            ActGlobals.oFormActMain.BeforeLogLineRead -= oFormActMain_BeforeLogLineRead;
            ActGlobals.oFormActMain.OnLogLineRead -= oFormActMain_OnLogLineRead;

            SaveSettings();
            browser.ExecuteScriptAsync("api_overlaywindow_close_all();");
            lblStatus.Text = "Plugin Exited";
        }

        #endregion

        void oFormActMain_AfterCombatAction(bool isImport, CombatActionEventArgs actionInfo)
        {
        }

        async void LoadSettings()
        {
            if (File.Exists(settingsFile))
            {
                JObject obj = new JObject();
                try {
                    String s = File.ReadAllText(settingsFile);
                    obj = JObject.Parse(s);
                    JToken token;
                    if (obj.TryGetValue("Port", out token))
                    {
                        Port = token.ToObject<int>();
                    }
                    else
                    {
                        Port = 10501;
                    }
                    if (obj.TryGetValue("UPnPPort", out token))
                    {
                        UPnPPort = token.ToObject<int>();
                    }
                    else
                    {
                        UPnPPort = Port;
                    }
                    if (obj.TryGetValue("Hostname", out token))
                    {
                        Hostname = token.ToObject<String>();
                    }
                    else
                    {
                        Hostname = "localhost";
                    }
                    if (obj.TryGetValue("RandomURL", out token))
                    {
                        RandomURL = token.ToObject<bool>();
                    }
                    else
                    {
                        RandomURL = false;
                    }
                    if (obj.TryGetValue("UseUPnP", out token))
                    {
                        UseUPnP = token.ToObject<bool>();
                    }
                    else
                    {
                        UseUPnP = false;
                    }
                    if (obj.TryGetValue("AutoRun", out token))
                    {
                        AutoRun = token.ToObject<bool>();
                    }
                    else
                    {
                        AutoRun = false;
                    }
                }
                catch (Exception e)
                {
                }
            }
        }

        void SaveSettings()
        {
            browser.ExecuteScriptAsync("updateWebsocketSettings();main._SaveSettings();");
        }

        public void _SaveSettings()
        {
            browser.ExecuteScriptAsync("");
            JObject obj = new JObject();
            obj.Add("Port", Port);
            obj.Add("UPnPPort", UPnPPort);
            obj.Add("Hostname", Hostname);
            obj.Add("RandomURL", RandomURL);
            obj.Add("UseUPnP", UseUPnP);
            obj.Add("AutoRun", AutoRun);
            String s = obj.ToString();
            File.WriteAllText(settingsFile, s);
        }

        public void LoadOverlaySettings()
        {
            if (File.Exists(settingsFile))
            {
                JObject obj = new JObject();
                try
                {
                    String s = File.ReadAllText(overlaySettingsFile);
                    obj = JObject.Parse(s);
                    browser.ExecuteScriptAsync("update_all(JSON.parse(Hex2Str(\"" + Utility.Str2Hex(s) + "\")));");
                }
                catch (Exception e)
                {
                }
            }
        }

        public void SaveOverlaySettings(String json)
        {
            try
            {
                JObject obj = JObject.Parse(json);
                File.WriteAllText(overlaySettingsFile, obj.ToString());
            }
            catch (Exception e)
            {

            }
        }

        private void ACTWebSocket_Load(object sender, EventArgs e)
        {
        }

        private void oFormActMain_BeforeLogLineRead(
            bool isImport,
            LogLineEventArgs logInfo)
        {
            FilterChatings("/BeforeLogLineRead", logInfo);
        }

        private void oFormActMain_OnLogLineRead(
            bool isImport,
            LogLineEventArgs logInfo)
        {
            FilterChatings("/OnLogLineRead", logInfo);
        }

        private void FilterChatings(string url, LogLineEventArgs e)
        {
            string[] data = e.logLine.Split('|');

            try
            {
                if (Convert.ToInt32(data[0]) == 0)
                {
                    if (Convert.ToInt32(data[2], 16) < 54 && ChatFilter)
                    {
                        return;
                    }
                }
            }catch(Exception)
            {
                // TODO ?
            }
            core.Broadcast(url, e.logLine);
        }

        #region Web JSObject Part
        public int Port { get; set; }
        public int UPnPPort { get; set; }
        public string Hostname { get; set; }
        public bool RandomURL { get; set; }
        public bool UseUPnP { get; set; }
        public bool AutoRun { get; set; }

        public void StartServer()
        {
            if(UseUPnP)
            {
                Task upnpTask = new Task(async () =>
                {
                    var discoverer = new NatDiscoverer();
                    var cts = new CancellationTokenSource(10000); // 10secs
                    var device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);

                    // not registered when first invoke...
                    await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, Port, UPnPPort, "ACTWebSocket Port"));
                    await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, Port, UPnPPort, "ACTWebSocket Port"));
                    await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, Port, UPnPPort, "ACTWebSocket Port"));
                });
                upnpTask.Start();
            }

            IPHostEntry hostEntry;
            IPAddress address;
            var addresses = Dns.GetHostAddresses(Hostname);

            bool localhostOnly = false;
            for(int i=0;i< addresses.Length;++i)
            {
                var ip = addresses[i];
                if (IPAddress.IsLoopback(ip))
                {
                    localhostOnly = true;
                    break;
                }
            }
            
            if (RandomURL)
            {
                core.randomDir = Guid.NewGuid().ToString();
            }
            else
            {
                core.randomDir = null;
            }
            try
            {
                if (UseUPnP)
                {
                    core.StartServer(localhostOnly ? "127.0.0.1" : "0.0.0.0", Port, UPnPPort, Hostname);
                }
                else
                {
                    core.StartServer(localhostOnly ? "127.0.0.1" : "0.0.0.0", Port, Port, Hostname);
                }
            }
            catch (Exception e)
            {
                browser.ExecuteScriptAsync("forceChange('[data-flag=serverstatus]');");
                MessageBox.Show(e.Message);
                core.StopServer();
            }
        }

        public void StopServer()
        {
            core.StopServer();
        }
        
        public void consolelog(object s)
        {
            Console.WriteLine(s);
        }
        
        public bool BeforeLogLineRead
        {
            get {
                if (core != null)
                {
                    return core.Filters["/BeforeLogLineRead"];
                }
                return false;
            }
            set
            {
                if (core != null)
                {
                    core.Filters["/BeforeLogLineRead"] = value;
                }
            }
        }

        public bool OnLogLineRead
        {
            get
            {
                if (core != null)
                {
                    return core.Filters["/OnLogLineRead"];
                }
                return false;
            }
            set
            {
                if (core != null)
                {
                    core.Filters["/OnLogLineRead"] = value;
                }
            }
        }

        public bool MiniParse 
        {
            get
            {
                if (core != null)
                {
                    return core.Filters["/MiniParse"];
                }
                return false;
            }
            set
            {
                if (core != null)
                {
                    core.Filters["/MiniParse"] = value;
                }
            }
        }

        public void GetSkinList()
        {
            string s = "<div data-url=\"\">{FILEURL}<div class=\"link\"></div><div class=\"select\"></div></div>";
            JObject o = new JObject();
            foreach (string file in Directory.EnumerateFiles(overlaySkinDirectory, "*.html", SearchOption.AllDirectories))
            {
                Console.WriteLine(file);
                o.Add(Utility.GetRelativePath(file, overlaySkinDirectory));
            }

            //return o.ToString();
        }

        public void Init()
        {
            LoadOverlaySettings();
            // MessageBox.Show("ACTWebSocket is Initialized :3");
        }

        public void openDEV()
        {
            browser.ShowDevTools();
        }

        public void copyURL(string skinPath = "")
        {
            string url = "";
            {
                url = "http://" + Hostname + ":" + Port + "/";
            }
            if (core.randomDir != null)
            {
                url += core.randomDir + "/";
            }
            if (skinPath.Length > 0)
            {
                string fullURL = url + Uri.EscapeDataString(skinPath);
                fullURL = fullURL.Replace("%5C", "/");
                Clipboard.SetText(fullURL);
            }
            else
            {
                Clipboard.SetText(url);
            }
        }
        #endregion Web JSObject Part End
    }
}
