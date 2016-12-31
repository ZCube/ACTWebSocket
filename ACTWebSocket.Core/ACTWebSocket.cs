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

    public interface PluginDirectory
    {
        void SetPluginDirectory(string path);
        string GetPluginDirectory();
    }
    public class ACTWebSocketMain : UserControl, IActPluginV1, PluginDirectory
    {
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
        //CEFWorks CEF;
        public void InitBrowser()
        {
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

            Controls.Add(browser);
            browser.RegisterJsObject("main", this);

            browser.Dock = DockStyle.Fill;

        }

        ~ACTWebSocketMain()
        {
            SaveSettings();
        }

        string overlayWindowPrefix = "_private_overlay_";

        Label lblStatus;    // The status label that appears in ACT's Plugin tab
        string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\ACTWebSocket.config.xml");
        SettingsSerializer xmlSettings;

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
            CultureInfo ci = CultureInfo.InstalledUICulture;
            string specName = "(none)";
            try { specName = CultureInfo.CreateSpecificCulture(ci.Name).Name; } catch { }
            if(specName == "ko-KR")
            {
                sortTypeDict = new List<KeyValuePair<string, MiniParseSortType>>()
                {
                    new KeyValuePair<string, MiniParseSortType>("정렬 안함", MiniParseSortType.None),
                    new KeyValuePair<string, MiniParseSortType>("문자열 올림차순 정렬", MiniParseSortType.StringAscending),
                    new KeyValuePair<string, MiniParseSortType>("문자열 내림차순 정렬", MiniParseSortType.StringDescending),
                    new KeyValuePair<string, MiniParseSortType>("숫자 올림차순 정렬", MiniParseSortType.NumericAscending),
                    new KeyValuePair<string, MiniParseSortType>("숫자 내림차순 정렬", MiniParseSortType.NumericDescending)
                };
            }
            else
            {
                sortTypeDict = new List<KeyValuePair<string, MiniParseSortType>>()
                {
                    new KeyValuePair<string, MiniParseSortType>("DoNotSort", MiniParseSortType.None),
                    new KeyValuePair<string, MiniParseSortType>("SortStringAscending", MiniParseSortType.StringAscending),
                    new KeyValuePair<string, MiniParseSortType>("SortStringDescending", MiniParseSortType.StringDescending),
                    new KeyValuePair<string, MiniParseSortType>("SortNumberAscending", MiniParseSortType.NumericAscending),
                    new KeyValuePair<string, MiniParseSortType>("SortNumberDescending", MiniParseSortType.NumericDescending)
                };
            }
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
            xmlSettings = new SettingsSerializer(this);	// Create a new settings serializer and pass it this instance

            //sortType.SelectedIndex = -1;
            //sortType.DataSource = sortTypeDict;
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
            lblStatus.Text = "Plugin Exited";
        }

        #endregion

        void oFormActMain_AfterCombatAction(bool isImport, CombatActionEventArgs actionInfo)
        {
        }

        async void LoadSettings()
        {
            //xmlSettings.AddControlSetting(port.Name, port);
            //xmlSettings.AddControlSetting(localhostOnly.Name, localhostOnly);
            //xmlSettings.AddControlSetting(autostart.Name, autostart);
            //xmlSettings.AddControlSetting(hostname.Name, hostname);
            //xmlSettings.AddControlSetting(MiniParseSortKey.Name, MiniParseSortKey);
            //xmlSettings.AddControlSetting(sortType.Name, sortType);
            //xmlSettings.AddControlSetting(MiniParseUse.Name, MiniParseUse);
            //xmlSettings.AddControlSetting(BeforeLogLineReadUse.Name, BeforeLogLineReadUse);
            //xmlSettings.AddControlSetting(randomURL.Name, randomURL);

            if (File.Exists(settingsFile))
            {
                FileStream fs = new FileStream(settingsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlTextReader xReader = new XmlTextReader(fs);

                try
                {
                    while (xReader.Read())
                    {
                        if (xReader.NodeType == XmlNodeType.Element)
                        {
                            if (xReader.LocalName == "SettingsSerializer")
                            {
                                xmlSettings.ImportFromXml(xReader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error loading settings: " + ex.Message;
                }
                xReader.Close();
            }

            // validate
            //try
            //{
            //    int p = Convert.ToInt16(port.Text);
            //}
            //catch (Exception e)
            //{
            //    port.Text = "10501";
            //}

            //if (hostname.Text.Length == 0)
            //{
            //    hostname.Text = "localhost";
            //}
            //UpdateList();
        }

        void SaveSettings()
        {
            FileStream fs = new FileStream(settingsFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            XmlTextWriter xWriter = new XmlTextWriter(fs, Encoding.UTF8);
            xWriter.Formatting = Formatting.Indented;
            xWriter.Indentation = 1;
            xWriter.IndentChar = '\t';
            xWriter.WriteStartDocument(true);
            xWriter.WriteStartElement("Config");    // <Config>
            xWriter.WriteStartElement("SettingsSerializer");    // <Config><SettingsSerializer>
            xmlSettings.ExportToXml(xWriter);   // Fill the SettingsSerializer XML
            xWriter.WriteEndElement();  // </SettingsSerializer>
            xWriter.WriteEndElement();  // </Config>
            xWriter.WriteEndDocument(); // Tie up loose ends (shouldn't be any)
            xWriter.Flush();    // Flush the file buffer to disk
            xWriter.Close();
        }
        
        private void ACTWebSocket_Load(object sender, EventArgs e)
        {
            //UpdateList();
        }

        private void oFormActMain_BeforeLogLineRead(
            bool isImport,
            LogLineEventArgs logInfo)
        {
            core.Broadcast("/BeforeLogLineRead", logInfo.logLine);
        }

        private void oFormActMain_OnLogLineRead(
            bool isImport,
            LogLineEventArgs logInfo)
        {
            core.Broadcast("/OnLogLineRead", logInfo.logLine);
        }

        public ushort Port { get; set; }
        public ushort UPnPPort { get; set; }
        public string Hostname { get; set; }
        public bool RandomURL { get; set; }
        public bool LocalhostOnly { get; set; }
        public bool UseUPNP { get; set; }

        private void StartServer()
        {
            if(UseUPNP)
            {
                Task upnpTask = new Task(async () =>
                {
                    var discoverer = new NatDiscoverer();
                    var cts = new CancellationTokenSource(10000); // 10secs
                    var device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);

                    await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, Port, UPnPPort, "ACTWebSocket Port"));
                });
                upnpTask.Start();
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
                if (UseUPNP)
                {
                    core.StartServer(LocalhostOnly ? "127.0.0.1" : "0.0.0.0", Port, UPnPPort, Hostname);
                }
                else
                {
                    core.StartServer(LocalhostOnly ? "127.0.0.1" : "0.0.0.0", Port, Port, Hostname);
                }
            }
            catch (Exception e)
            {
                browser.ExecuteScriptAsync("forceChange('[data-flag=serverstatus]');");
                MessageBox.Show(e.Message);
                core.StopServer();
            }
        }

        private void StopServer()
        {
            core.StopServer();
        }

        public void buttonOn_Click()
        {
            StartServer();
        }

        public void buttonOff_Click()
        {
            StopServer();
        }

        public void consolelog(object s)
        {
            Console.WriteLine(s);
        }
        
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
            MessageBox.Show("ACTWebSocket is Initialized :3");
        }

        private void copyURL(string skinPath = "")
        {
            string url = "";
            if (LocalhostOnly)
            {
                url = "http://localhost:" + Port + "/";
            }
            else
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
    }
}
