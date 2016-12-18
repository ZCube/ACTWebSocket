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
    using System.Linq;
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
        void SetPluginDirectory(String path);
        String GetPluginDirectory();
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
        private Button copyURL;
        private CheckBox randomURL;
        private Button button1;
        private Button button2;
        private ListBox listBox2;
        private ListBox listBox1;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private Button button3;
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
        private GroupBox groupBox1;
        private SplitContainer splitContainer1;
        private Button button4;
        private GroupBox groupBox2;
        private CheckBox checkBox6;
        private CheckBox checkBox5;
        private CheckBox checkBox4;
        private CheckBox checkBox3;
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
            this.copyURL = new System.Windows.Forms.Button();
            this.localhostOnly = new System.Windows.Forms.CheckBox();
            this.hostname = new System.Windows.Forms.TextBox();
            this.randomURL = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.startoption.SuspendLayout();
            this.hostdata.SuspendLayout();
            this.miniparse.SuspendLayout();
            this.othersets.SuspendLayout();
            this.serverStatus.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.MiniParseUse.Checked = true;
            this.MiniParseUse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MiniParseUse.Name = "MiniParseUse";
            this.MiniParseUse.UseVisualStyleBackColor = true;
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
            this.BeforeLogLineReadUse.Name = "BeforeLogLineReadUse";
            this.BeforeLogLineReadUse.UseVisualStyleBackColor = true;
            this.BeforeLogLineReadUse.CheckedChanged += new System.EventHandler(this.LogLineReadUse_CheckedChanged);
            // 
            // OnLogLineReadUse
            // 
            resources.ApplyResources(this.OnLogLineReadUse, "OnLogLineReadUse");
            this.OnLogLineReadUse.Name = "OnLogLineReadUse";
            this.OnLogLineReadUse.UseVisualStyleBackColor = true;
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
            // copyURL
            // 
            resources.ApplyResources(this.copyURL, "copyURL");
            this.copyURL.Name = "copyURL";
            this.copyURL.UseVisualStyleBackColor = true;
            this.copyURL.Click += new System.EventHandler(this.copyURL_Click);
            // 
            // localhostOnly
            // 
            resources.ApplyResources(this.localhostOnly, "localhostOnly");
            this.localhostOnly.Checked = true;
            this.localhostOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.localhostOnly.Name = "localhostOnly";
            this.localhostOnly.UseVisualStyleBackColor = true;
            // 
            // hostname
            // 
            resources.ApplyResources(this.hostname, "hostname");
            this.hostname.Name = "hostname";
            // 
            // randomURL
            // 
            resources.ApplyResources(this.randomURL, "randomURL");
            this.randomURL.Name = "randomURL";
            this.randomURL.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listBox2
            // 
            resources.ApplyResources(this.listBox2, "listBox2");
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Name = "listBox2";
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // listBox1
            // 
            resources.ApplyResources(this.listBox1, "listBox1");
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Name = "listBox1";
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            resources.ApplyResources(this.checkBox2, "checkBox2");
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // startoption
            // 
            this.startoption.Controls.Add(this.localhostOnly);
            this.startoption.Controls.Add(this.randomURL);
            this.startoption.Controls.Add(this.autostart);
            resources.ApplyResources(this.startoption, "startoption");
            this.startoption.Name = "startoption";
            this.startoption.TabStop = false;
            // 
            // hostdata
            // 
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
            this.label15.Name = "label15";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // miniparse
            // 
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
            this.label17.Name = "label17";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // othersets
            // 
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
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.splitContainer1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button4);
            this.splitContainer1.Panel1.Controls.Add(this.listBox1);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Panel1.Controls.Add(this.copyURL);
            this.splitContainer1.Panel1.Controls.Add(this.button2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.Controls.Add(this.listBox2);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox6);
            this.groupBox2.Controls.Add(this.checkBox5);
            this.groupBox2.Controls.Add(this.checkBox4);
            this.groupBox2.Controls.Add(this.checkBox3);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Controls.Add(this.button3);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // checkBox3
            // 
            resources.ApplyResources(this.checkBox3, "checkBox3");
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            resources.ApplyResources(this.checkBox4, "checkBox4");
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            resources.ApplyResources(this.checkBox5, "checkBox5");
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            resources.ApplyResources(this.checkBox6, "checkBox6");
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // ACTWebSocketMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox1);
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
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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

        Label lblStatus;    // The status label that appears in ACT's Plugin tab
        string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\ACTWebSocket.config.xml");
        SettingsSerializer xmlSettings;

        #region IActPluginV1 Members

        static readonly List<KeyValuePair<string, MiniParseSortType>> sortTypeDict = new List<KeyValuePair<string, MiniParseSortType>>()
        {
            new KeyValuePair<string, MiniParseSortType>("DoNotSort", MiniParseSortType.None),
            new KeyValuePair<string, MiniParseSortType>("SortStringAscending", MiniParseSortType.StringAscending),
            new KeyValuePair<string, MiniParseSortType>("SortStringDescending", MiniParseSortType.StringDescending),
            new KeyValuePair<string, MiniParseSortType>("SortNumberAscending", MiniParseSortType.NumericAscending),
            new KeyValuePair<string, MiniParseSortType>("SortNumberDescending", MiniParseSortType.NumericDescending)
        };

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            if (core == null)
            {
                core = new ACTWebSocketCore();
                core.pluginDirectory = pluginDirectory;
            }
            lblStatus = pluginStatusText;   // Hand the status label's reference to our local var
            pluginScreenSpace.Controls.Add(this);   // Add this UserControl to the tab ACT provides
            this.Dock = DockStyle.Fill; // Expand the UserControl to fill the tab's client space
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
            if (autostart.Checked)
            {
                StartServer();
            }
            else
            {
                StopServer();
            }
            // Create some sort of parsing event handler.  After the "+=" hit TAB twice and the code will be generated for you.
            ActGlobals.oFormActMain.BeforeLogLineRead += this.oFormActMain_BeforeLogLineRead;
            ActGlobals.oFormActMain.OnLogLineRead += this.oFormActMain_OnLogLineRead;
            var s = ActGlobals.oFormActMain.ActPlugins;
            lblStatus.Text = "Plugin Started";
        }

        public void DeInitPlugin()
        {
            StopServer();
            // Unsubscribe from any events you listen to when exiting!
            ActGlobals.oFormActMain.BeforeLogLineRead -= this.oFormActMain_BeforeLogLineRead;
            ActGlobals.oFormActMain.OnLogLineRead -= this.oFormActMain_OnLogLineRead;

            CloseAll();
            SaveSettings();
            lblStatus.Text = "Plugin Exited";
        }

        #endregion

        void oFormActMain_AfterCombatAction(bool isImport, CombatActionEventArgs actionInfo)
        {
        }

        void LoadSettings()
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

        String pluginDirectory = "";
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

        string GetRelativePath(string filespec, string folder)
        {
            Uri pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        public void CloseAll()
        {
            UpdateList();
            for(int i=0;i<listBox2.Items.Count;++i)
            {
                String title = this.listBox2.Items[i].ToString();
                IntPtr hwnd = Native.FindWindow(null, title);
                if (hwnd == null || hwnd.ToInt64() == 0)
                {
                }
                else
                {
                    Native.SendMessage(hwnd, 0x0400 + 1, new IntPtr(0x08), new IntPtr(0x08));
                    Native.CloseWindow(hwnd);
                }
            }
        }
        public void UpdateList()
        {
            this.listBox1.Items.Clear();
            foreach (string file in Directory.EnumerateFiles(pluginDirectory, "*.html", SearchOption.AllDirectories))
            {
                this.listBox1.Items.Add(GetRelativePath(file, pluginDirectory));
            }
            List<String> titles = Native.SearchForWindow("overlay_");
            this.listBox2.Items.Clear();
            this.listBox2.Sorted = true;
            foreach (string title in titles)
            {
                this.listBox2.Items.Add(title);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateList();
        }

        public static string Base64Encoding(string EncodingText, System.Text.Encoding oEncoding = null)
        {
            if (oEncoding == null)
                oEncoding = System.Text.Encoding.UTF8;

            byte[] arr = oEncoding.GetBytes(EncodingText);
            return System.Convert.ToBase64String(arr);
        }

        public static string Base64Decoding(string DecodingText, System.Text.Encoding oEncoding = null)
        {
            if (oEncoding == null)
                oEncoding = System.Text.Encoding.UTF8;

            byte[] arr = System.Convert.FromBase64String(DecodingText);
            return oEncoding.GetString(arr);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String overlayPath = pluginDirectory + "/overlay/overlaydemo.exe";
            if (File.Exists(overlayPath) && this.listBox1.SelectedIndex >= 0)
            {
                String url = "";
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
                String param = url + System.Uri.EscapeDataString(this.listBox1.Items[this.listBox1.SelectedIndex].ToString());
                param = param.Replace("%5C", "/");
                String title = "overlay_" + Guid.NewGuid().ToString();
                this.listBox2.Items.Add(title);
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = overlayPath;
                String json = "";
                JObject o = new JObject();
                o["useDragFilter"] = true;
                o["useDragMove"] = true;
                o["hide"] = false;
                //o["width"] = 100;
                //o["height"] = 100;
                //o["x"] = 0;
                //o["y"] = 0;
                o["Transparent"] = true;
                o["NoActivate"] = true;
                json = o.ToString();
                startInfo.Arguments = param + " " + title + " " + Base64Encoding(json);
                Process.Start(startInfo);
            }
        }

        private void copyURL_Click(object sender, EventArgs e)
        {
            String url = "";
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
            if (this.listBox1.SelectedIndex >= 0)
            {
                String fullURL = url + System.Uri.EscapeDataString(this.listBox1.Items[this.listBox1.SelectedIndex].ToString());
                fullURL = fullURL.Replace("%5C", "/");
                Clipboard.SetText(fullURL);
            }
            else
            {
                Clipboard.SetText(url);
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.listBox2.SelectedIndex >= 0)
            {
                String title = this.listBox2.Items[this.listBox2.SelectedIndex].ToString();
                IntPtr hwnd = Native.FindWindow(null, title);
                if (hwnd == null || hwnd.ToInt64() == 0)
                {
                    UpdateList();
                }
                else
                {
                    JObject o = new JObject();
                    o["Transparent"] = checkBox1.Checked;
                    o["NoActivate"] = checkBox2.Checked;
                    String json = o.ToString();
                    Native.SendMessageToWindow(hwnd, 1, json);
                }
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.listBox2.SelectedIndex >= 0)
            {
                String title = this.listBox2.Items[this.listBox2.SelectedIndex].ToString();
                IntPtr hwnd = Native.FindWindow(null, title);
                if (hwnd == null || hwnd.ToInt64() == 0)
                {
                    //this.listBox2.Items.RemoveAt(this.listBox2.SelectedIndex);
                    UpdateList();
                }
                else
                {
                    JObject o = new JObject();
                    o["Transparent"] = checkBox1.Checked;
                    o["NoActivate"] = checkBox2.Checked;
                    String json = o.ToString();
                    Native.SendMessageToWindow(hwnd, 1, json);
                }
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox2.SelectedIndex >= 0)
            {
                String title = this.listBox2.Items[this.listBox2.SelectedIndex].ToString();
                IntPtr hwnd = Native.FindWindow(null, title);
                if (hwnd == null || hwnd.ToInt64() == 0)
                {
                    //this.listBox2.Items.RemoveAt(this.listBox2.SelectedIndex);
                    UpdateList();
                }
                else
                {
                    checkBox1.Checked = (Native.GetWindowLongFlag(hwnd, new IntPtr(Native.WS_EX_TRANSPARENT)).ToInt64() > 0);
                    checkBox2.Checked = (Native.GetWindowLongFlag(hwnd, new IntPtr(Native.WS_EX_NOACTIVATE)).ToInt64() > 0);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.listBox2.SelectedIndex >= 0)
            {
                String title = this.listBox2.Items[this.listBox2.SelectedIndex].ToString();
                IntPtr hwnd = Native.FindWindow(null, title);
                if (hwnd == null || hwnd.ToInt64() == 0)
                {
                    //this.listBox2.Items.RemoveAt(this.listBox2.SelectedIndex);
                    UpdateList();
                }
                else
                {
                    Native.SendMessage(hwnd, 0x0400 + 1, new IntPtr(0x08), new IntPtr(0x08));
                    Native.CloseWindow(hwnd);
                    this.listBox2.Items.RemoveAt(this.listBox2.SelectedIndex);
                }
            }
        }
    }
}
