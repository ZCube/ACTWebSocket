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

    public interface PluginDirectory
    {
        void SetPluginDirectory(string path);
        string GetPluginDirectory();
    }
    public class ACTWebSocketMain : UserControl, IActPluginV1, PluginDirectory
    {
        private ACTWebSocketCore core;
        private TextBox port;
        private CheckBox autostart;
        private CheckBox MiniParseUse;
        private TextBox MiniParseSortKey;
        private ComboBox sortType;
        private CheckBox BeforeLogLineReadUse;
        private CheckBox localhostOnly;
        private TextBox hostname;
        private CheckBox OnLogLineReadUse;
        private Button buttonOff;
        private Button buttonOn;
        private CheckBox randomURL;
        private Label label13;
        private GroupBox startoption;
        private GroupBox hostdata;
        private Label label15;
        private Label label14;
        private GroupBox miniparse;
        private Label label17;
        private Label label16;
        private GroupBox othersets;
        private GroupBox serverStatus;
        private Button copyURL;
        private Button button1;
        private GroupBox groupBox1;
        private ListBox listBox1;
        private Label label1;
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
            this.port = new System.Windows.Forms.TextBox();
            this.autostart = new System.Windows.Forms.CheckBox();
            this.MiniParseUse = new System.Windows.Forms.CheckBox();
            this.MiniParseSortKey = new System.Windows.Forms.TextBox();
            this.sortType = new System.Windows.Forms.ComboBox();
            this.BeforeLogLineReadUse = new System.Windows.Forms.CheckBox();
            this.OnLogLineReadUse = new System.Windows.Forms.CheckBox();
            this.buttonOff = new System.Windows.Forms.Button();
            this.buttonOn = new System.Windows.Forms.Button();
            this.localhostOnly = new System.Windows.Forms.CheckBox();
            this.hostname = new System.Windows.Forms.TextBox();
            this.randomURL = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.startoption = new System.Windows.Forms.GroupBox();
            this.hostdata = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.miniparse = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.othersets = new System.Windows.Forms.GroupBox();
            this.serverStatus = new System.Windows.Forms.GroupBox();
            this.copyURL = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.startoption.SuspendLayout();
            this.hostdata.SuspendLayout();
            this.miniparse.SuspendLayout();
            this.othersets.SuspendLayout();
            this.serverStatus.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // port
            // 
            resources.ApplyResources(this.port, "port");
            this.port.Name = "port";
            // 
            // autostart
            // 
            resources.ApplyResources(this.autostart, "autostart");
            this.autostart.BackColor = System.Drawing.Color.Transparent;
            this.autostart.Name = "autostart";
            this.autostart.UseVisualStyleBackColor = false;
            // 
            // MiniParseUse
            // 
            resources.ApplyResources(this.MiniParseUse, "MiniParseUse");
            this.MiniParseUse.BackColor = System.Drawing.Color.Transparent;
            this.MiniParseUse.Checked = true;
            this.MiniParseUse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MiniParseUse.Name = "MiniParseUse";
            this.MiniParseUse.UseVisualStyleBackColor = false;
            this.MiniParseUse.CheckedChanged += new System.EventHandler(this.MiniParseUse_CheckedChanged);
            // 
            // MiniParseSortKey
            // 
            resources.ApplyResources(this.MiniParseSortKey, "MiniParseSortKey");
            this.MiniParseSortKey.Name = "MiniParseSortKey";
            this.MiniParseSortKey.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // sortType
            // 
            resources.ApplyResources(this.sortType, "sortType");
            this.sortType.Cursor = System.Windows.Forms.Cursors.Default;
            this.sortType.DisplayMember = "Key";
            this.sortType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sortType.FormattingEnabled = true;
            this.sortType.Name = "sortType";
            this.sortType.ValueMember = "Value";
            this.sortType.SelectedIndexChanged += new System.EventHandler(this.MiniParseSortType_SelectedIndexChanged);
            // 
            // BeforeLogLineReadUse
            // 
            resources.ApplyResources(this.BeforeLogLineReadUse, "BeforeLogLineReadUse");
            this.BeforeLogLineReadUse.BackColor = System.Drawing.Color.Transparent;
            this.BeforeLogLineReadUse.Name = "BeforeLogLineReadUse";
            this.BeforeLogLineReadUse.UseVisualStyleBackColor = false;
            this.BeforeLogLineReadUse.CheckedChanged += new System.EventHandler(this.LogLineReadUse_CheckedChanged);
            // 
            // OnLogLineReadUse
            // 
            resources.ApplyResources(this.OnLogLineReadUse, "OnLogLineReadUse");
            this.OnLogLineReadUse.BackColor = System.Drawing.Color.Transparent;
            this.OnLogLineReadUse.Name = "OnLogLineReadUse";
            this.OnLogLineReadUse.UseVisualStyleBackColor = false;
            this.OnLogLineReadUse.CheckedChanged += new System.EventHandler(this.OnLogLineReadUse_CheckedChanged);
            // 
            // buttonOff
            // 
            resources.ApplyResources(this.buttonOff, "buttonOff");
            this.buttonOff.Name = "buttonOff";
            this.buttonOff.UseVisualStyleBackColor = true;
            this.buttonOff.Click += new System.EventHandler(this.buttonOff_Click);
            // 
            // buttonOn
            // 
            resources.ApplyResources(this.buttonOn, "buttonOn");
            this.buttonOn.Name = "buttonOn";
            this.buttonOn.UseVisualStyleBackColor = true;
            this.buttonOn.Click += new System.EventHandler(this.buttonOn_Click);
            // 
            // localhostOnly
            // 
            resources.ApplyResources(this.localhostOnly, "localhostOnly");
            this.localhostOnly.BackColor = System.Drawing.Color.Transparent;
            this.localhostOnly.Checked = true;
            this.localhostOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.localhostOnly.Name = "localhostOnly";
            this.localhostOnly.UseVisualStyleBackColor = false;
            // 
            // hostname
            // 
            resources.ApplyResources(this.hostname, "hostname");
            this.hostname.Name = "hostname";
            // 
            // randomURL
            // 
            resources.ApplyResources(this.randomURL, "randomURL");
            this.randomURL.BackColor = System.Drawing.Color.Transparent;
            this.randomURL.Name = "randomURL";
            this.randomURL.UseVisualStyleBackColor = false;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // startoption
            // 
            this.startoption.BackColor = System.Drawing.Color.Transparent;
            this.startoption.Controls.Add(this.localhostOnly);
            this.startoption.Controls.Add(this.randomURL);
            this.startoption.Controls.Add(this.autostart);
            resources.ApplyResources(this.startoption, "startoption");
            this.startoption.Name = "startoption";
            this.startoption.TabStop = false;
            // 
            // hostdata
            // 
            this.hostdata.BackColor = System.Drawing.Color.Transparent;
            this.hostdata.Controls.Add(this.label15);
            this.hostdata.Controls.Add(this.label14);
            this.hostdata.Controls.Add(this.hostname);
            this.hostdata.Controls.Add(this.port);
            resources.ApplyResources(this.hostdata, "hostdata");
            this.hostdata.Name = "hostdata";
            this.hostdata.TabStop = false;
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Name = "label15";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Name = "label14";
            // 
            // miniparse
            // 
            this.miniparse.BackColor = System.Drawing.Color.Transparent;
            this.miniparse.Controls.Add(this.label17);
            this.miniparse.Controls.Add(this.label16);
            this.miniparse.Controls.Add(this.MiniParseUse);
            this.miniparse.Controls.Add(this.sortType);
            this.miniparse.Controls.Add(this.MiniParseSortKey);
            resources.ApplyResources(this.miniparse, "miniparse");
            this.miniparse.Name = "miniparse";
            this.miniparse.TabStop = false;
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Name = "label17";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Name = "label16";
            // 
            // othersets
            // 
            this.othersets.BackColor = System.Drawing.Color.Transparent;
            this.othersets.Controls.Add(this.BeforeLogLineReadUse);
            this.othersets.Controls.Add(this.OnLogLineReadUse);
            resources.ApplyResources(this.othersets, "othersets");
            this.othersets.Name = "othersets";
            this.othersets.TabStop = false;
            // 
            // serverStatus
            // 
            this.serverStatus.Controls.Add(this.buttonOn);
            this.serverStatus.Controls.Add(this.buttonOff);
            resources.ApplyResources(this.serverStatus, "serverStatus");
            this.serverStatus.Name = "serverStatus";
            this.serverStatus.TabStop = false;
            // 
            // copyURL
            // 
            resources.ApplyResources(this.copyURL, "copyURL");
            this.copyURL.Name = "copyURL";
            this.copyURL.UseVisualStyleBackColor = true;
            this.copyURL.Click += new System.EventHandler(this.copyURL_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // listBox1
            // 
            resources.ApplyResources(this.listBox1, "listBox1");
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Name = "listBox1";
            // 
            // ACTWebSocketMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.copyURL);
            this.Controls.Add(this.serverStatus);
            this.Controls.Add(this.othersets);
            this.Controls.Add(this.miniparse);
            this.Controls.Add(this.hostdata);
            this.Controls.Add(this.startoption);
            this.Controls.Add(this.label13);
            resources.ApplyResources(this, "$this");
            this.Name = "ACTWebSocketMain";
            this.Load += new System.EventHandler(this.ACTWebSocket_Load);
            this.startoption.ResumeLayout(false);
            this.startoption.PerformLayout();
            this.hostdata.ResumeLayout(false);
            this.hostdata.PerformLayout();
            this.miniparse.ResumeLayout(false);
            this.miniparse.PerformLayout();
            this.othersets.ResumeLayout(false);
            this.othersets.PerformLayout();
            this.serverStatus.ResumeLayout(false);
            this.serverStatus.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        #endregion
        public ACTWebSocketMain()
        {
            InitializeComponent();
        }

        ~ACTWebSocketMain()
        {
            CloseAll();
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
            sortType.SelectedIndex = -1;

            sortType.DataSource = sortTypeDict;
            LoadSettings();



            if (core != null)
            {
                core.Filters["/BeforeLogLineRead"] = BeforeLogLineReadUse.Checked;
                core.Filters["/OnLogLineRead"] = OnLogLineReadUse.Checked;
                core.Filters["/MiniParse"] = MiniParseUse.Checked;
                core.Config.SortKey = MiniParseSortKey.Text.Trim();
                core.Config.SortType = (MiniParseSortType)sortType.SelectedIndex;
            }
            try
            {
                core.StartUIServer();

                if (autostart.Checked)
                {
                    StartServer();
                }
                else
                {
                    StopServer();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                core.StopUIServer();
                StopServer();
            }
            // Create some sort of parsing event handler.  After the "+=" hit TAB twice and the code will be generated for you.
            ActGlobals.oFormActMain.BeforeLogLineRead += oFormActMain_BeforeLogLineRead;
            ActGlobals.oFormActMain.OnLogLineRead += oFormActMain_OnLogLineRead;
            var s = ActGlobals.oFormActMain.ActPlugins;
            lblStatus.Text = "Plugin Started";
        }

        public void DeInitPlugin()
        {
            StopServer();
            // Unsubscribe from any events you listen to when exiting!
            ActGlobals.oFormActMain.BeforeLogLineRead -= oFormActMain_BeforeLogLineRead;
            ActGlobals.oFormActMain.OnLogLineRead -= oFormActMain_OnLogLineRead;

            SaveSettings();
            CloseAll();
            lblStatus.Text = "Plugin Exited";
        }

        #endregion

        void oFormActMain_AfterCombatAction(bool isImport, CombatActionEventArgs actionInfo)
        {
        }

        async void LoadSettings()
        {
            xmlSettings.AddControlSetting(port.Name, port);
            xmlSettings.AddControlSetting(localhostOnly.Name, localhostOnly);
            xmlSettings.AddControlSetting(autostart.Name, autostart);
            xmlSettings.AddControlSetting(hostname.Name, hostname);
            xmlSettings.AddControlSetting(MiniParseSortKey.Name, MiniParseSortKey);
            xmlSettings.AddControlSetting(sortType.Name, sortType);
            xmlSettings.AddControlSetting(MiniParseUse.Name, MiniParseUse);
            xmlSettings.AddControlSetting(BeforeLogLineReadUse.Name, BeforeLogLineReadUse);
            xmlSettings.AddControlSetting(randomURL.Name, randomURL);

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
            try
            {
                int p = Convert.ToInt16(port.Text);
            }
            catch (Exception e)
            {
                port.Text = "10501";
            }

            if (hostname.Text.Length == 0)
            {
                hostname.Text = "localhost";
            }
            UpdateList();
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

            IList<string> keys = overlayWindows.Properties().Select(p => p.Name).ToList();
            foreach (string title in keys)
            {
                IntPtr hwnd = Native.FindWindow(null, overlayWindowPrefix + title);
                if (hwnd == null || hwnd.ToInt64() == 0)
                {
                }
                else
                {
                    Native.RECT rect = new Native.RECT();
                    if(Native.GetWindowRect(hwnd, out rect))
                    {
                        overlayWindows[title]["x"] = rect.Left;
                        overlayWindows[title]["y"] = rect.Top;
                        overlayWindows[title]["width"] = rect.Right - rect.Left;
                        overlayWindows[title]["height"] = rect.Bottom - rect.Top;
                    }
                }
            }
            File.WriteAllText(pluginDirectory+ "\\overlayconfig.json", overlayWindows.ToString());
        }
        
        private void ACTWebSocket_Load(object sender, EventArgs e)
        {
            UpdateList();
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


        private void StartServer()
        {
            if (randomURL.Checked)
            {
                core.randomDir = Guid.NewGuid().ToString();
            }
            else
            {
                core.randomDir = null;
            }
            try
            {
                core.StartServer(localhostOnly.Checked ? "127.0.0.1" : "0.0.0.0", Convert.ToInt16(port.Text), hostname.Text.Trim());
                localhostOnly.Enabled = false;
                port.Enabled = false;
                hostname.Enabled = false;
                buttonOn.Enabled = false;
                buttonOff.Enabled = true;
                randomURL.Enabled = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                core.StopServer();
            }
            //tabPage1
        }

        private void StopServer()
        {
            core.StopServer();
            localhostOnly.Enabled = true;
            port.Enabled = true;
            hostname.Enabled = true;
            buttonOn.Enabled = true;
            buttonOff.Enabled = false;
            randomURL.Enabled = true;
        }

        private void buttonOn_Click(object sender, EventArgs e)
        {
            StartServer();
        }

        private void buttonOff_Click(object sender, EventArgs e)
        {
            StopServer();
        }

        private void port_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        string overlaySkinDirectory { get; set; }
        string pluginDirectory = "";
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (core != null)
            {
                core.Config.SortKey = MiniParseSortKey.Text.Trim();
            }
        }

        private void MiniParseSortType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (core != null)
            {
                core.Config.SortType = (MiniParseSortType)sortType.SelectedIndex;
            }
        }

        private void LogLineReadUse_CheckedChanged(object sender, EventArgs e)
        {
            if (core != null)
            {
                core.Filters["/BeforeLogLineRead"] = BeforeLogLineReadUse.Checked;
            }
        }

        private void OnLogLineReadUse_CheckedChanged(object sender, EventArgs e)
        {
            if (core != null)
            {
                core.Filters["/OnLogLineRead"] = OnLogLineReadUse.Checked;
            }
        }

        private void MiniParseUse_CheckedChanged(object sender, EventArgs e)
        {
            if (core != null)
            {
                core.Filters["/MiniParse"] = MiniParseUse.Checked;
            }
        }

        public void CloseAll()
        {
            UpdateList(false);
        }

        public void UpdateList(bool updateInfo = true)
        {
            listBox1.Items.Clear();
            foreach (string file in Directory.EnumerateFiles(overlaySkinDirectory, "*.html", SearchOption.AllDirectories))
            {
                listBox1.Items.Add(Utility.GetRelativePath(file, overlaySkinDirectory));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            copyURL_Click(sender, new EventArgs());
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        private void copyURL_Click(object sender, EventArgs e)
        {
            string url = "";
            if (localhostOnly.Checked)
            {
                url = "http://localhost:" + port.Text + "/";
            }
            else
            {
                url = "http://" + hostname.Text + ":" + port.Text + "/";
            }
            if (core.randomDir != null)
            {
                url += core.randomDir + "/";
            }
            if (listBox1.SelectedIndex >= 0)
            {
                string fullURL = url + Uri.EscapeDataString(listBox1.Items[listBox1.SelectedIndex].ToString());
                fullURL = fullURL.Replace("%5C", "/");
                Clipboard.SetText(fullURL);
            }
            else
            {
                Clipboard.SetText(url);
            }
        }

        private void digitOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
