using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using System.IO;
using System.Reflection;
using System.Xml;

namespace ACTWebSocket_Plugin
{
    using ACTWebSocket.Core;
    using CefSharp;
    using CefSharp.WinForms;
    using Classes;
    using Newtonsoft.Json.Linq;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using System.Timers;
    using WebSocketSharp;
    using WebSocketSharp.Net;
    using WebSocketSharp.Server;
    using static ACTWebSocketCore;
    using CefSharp;
    using System.IO;
    using System.Reflection;
    using ACTWebSocket.Core.Classes.Interfaces;

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
            settings.ResourcesDirPath = Settings.CEFDIR;
            settings.UserDataPath = Settings.CEFUSR;
            settings.CachePath = Settings.CEFBIN;
            settings.LogFile = Settings.LOGDIR + "\\debug.log";

            Cef.Initialize(settings);

            browser = new ChromiumWebBrowser("rsrc://localhost/Resources/MainForm.html");

            Controls.Add(browser);


            //panel1.Dock = DockStyle.Top;
            //panel1.Height = 32;
            //panel1.BringToFront();
            //panel1.Click += Panel1_Click;
            //panel1.MouseDown += Panel1_MouseDown;
            //panel1.MouseUp += Panel1_MouseUp;
            //panel1.MouseMove += Panel1_MouseMove;
            //panel1.MaximumSize = new Size(Width - 48, 48);

            //browser.SendToBack();
            //browser.ConsoleMessage += Browser_ConsoleMessage;
            //browser.MenuHandler = new CustomMenuHandler();
            //browser.ContextMenu = new ContextMenu();
            browser.Dock = DockStyle.Fill;

            //CEF = new CEFWorks(browser, this);

            //wc = new WebClient();
            //wc.DownloadFileCompleted += Wc_DownloadFileCompleted;
            //wc.DownloadProgressChanged += Wc_DownloadProgressChanged;
            //safejs = new SafeJSExecute(SafeJSExecuteAsync);
            //try
            //{
            //    //Console.WriteLine(Monitor.Core.Utilities.JunctionPoint.GetTarget(actpath));
            //    existact = true;
            //}
            //catch { }
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


        //private void StartServer()
        //{
        //    if (randomURL.Checked)
        //    {
        //        core.randomDir = Guid.NewGuid().ToString();
        //    }
        //    else
        //    {
        //        core.randomDir = null;
        //    }
        //    try
        //    {
        //        core.StartServer(localhostOnly.Checked ? "127.0.0.1" : "0.0.0.0", Convert.ToInt16(port.Text), hostname.Text.Trim());
        //        localhostOnly.Enabled = false;
        //        port.Enabled = false;
        //        hostname.Enabled = false;
        //        buttonOn.Enabled = false;
        //        buttonOff.Enabled = true;
        //        randomURL.Enabled = false;
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //        core.StopServer();
        //    }
        //    //tabPage1
        //}

        //private void StopServer()
        //{
        //    core.StopServer();
        //    localhostOnly.Enabled = true;
        //    port.Enabled = true;
        //    hostname.Enabled = true;
        //    buttonOn.Enabled = true;
        //    buttonOff.Enabled = false;
        //    randomURL.Enabled = true;
        //}

        //private void buttonOn_Click(object sender, EventArgs e)
        //{
        //    StartServer();
        //}

        //private void buttonOff_Click(object sender, EventArgs e)
        //{
        //    StopServer();
        //}

        //private void port_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        //        (e.KeyChar != '.'))
        //    {
        //        e.Handled = true;
        //    }

        //    // only allow one decimal point
        //    if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
        //    {
        //        e.Handled = true;
        //    }
        //}

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

        //private void textBox1_TextChanged(object sender, EventArgs e)
        //{
        //    if (core != null)
        //    {
        //        core.Config.SortKey = MiniParseSortKey.Text.Trim();
        //    }
        //}

        //private void MiniParseSortType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (core != null)
        //    {
        //        core.Config.SortType = (MiniParseSortType)sortType.SelectedIndex;
        //    }
        //}

        //private void LogLineReadUse_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (core != null)
        //    {
        //        core.Filters["/BeforeLogLineRead"] = BeforeLogLineReadUse.Checked;
        //    }
        //}

        //private void OnLogLineReadUse_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (core != null)
        //    {
        //        core.Filters["/OnLogLineRead"] = OnLogLineReadUse.Checked;
        //    }
        //}

        //private void MiniParseUse_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (core != null)
        //    {
        //        core.Filters["/MiniParse"] = MiniParseUse.Checked;
        //    }
        //}

        //public void CloseAll()
        //{
        //    UpdateList(false);
        //}

        //public void UpdateList(bool updateInfo = true)
        //{
        //    listBox1.Items.Clear();
        //    foreach (string file in Directory.EnumerateFiles(overlaySkinDirectory, "*.html", SearchOption.AllDirectories))
        //    {
        //        listBox1.Items.Add(Utility.GetRelativePath(file, overlaySkinDirectory));
        //    }
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    UpdateList();
        //}

        //private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    copyURL_Click(sender, new EventArgs());
        //}

        //[DllImport("user32.dll", SetLastError = true)]
        //public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        //private void copyURL_Click(object sender, EventArgs e)
        //{
        //    string url = "";
        //    if (localhostOnly.Checked)
        //    {
        //        url = "http://localhost:" + port.Text + "/";
        //    }
        //    else
        //    {
        //        url = "http://" + hostname.Text + ":" + port.Text + "/";
        //    }
        //    if (core.randomDir != null)
        //    {
        //        url += core.randomDir + "/";
        //    }
        //    if (listBox1.SelectedIndex >= 0)
        //    {
        //        string fullURL = url + Uri.EscapeDataString(listBox1.Items[listBox1.SelectedIndex].ToString());
        //        fullURL = fullURL.Replace("%5C", "/");
        //        Clipboard.SetText(fullURL);
        //    }
        //    else
        //    {
        //        Clipboard.SetText(url);
        //    }
        //}

        //private void digitOnly_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        //}
    }
}
